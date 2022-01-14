using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class WanderingEnemy : BaseEnemy
{
    [SerializeField] Vector3 wanderingAreaCenter;

    [SerializeField] float wanderingAreaSize;

    [SerializeField] float playerCheckRadius;

    [SerializeField] float wanderingSpeed;
    [SerializeField] float chasingSpeed;

    [SerializeField] float rotationSpeed;

    [SerializeField] float targetDistance;

    [SerializeField] float waitTimeToNewTarget;

    [SerializeField] Vector3 currentTarget;

    Transform player;

    bool isChasingPlayer = false;

    bool isMoving = false;

    Rigidbody rb2;

    void Start()
    {
        wanderingAreaCenter = transform.position;
        player = FindObjectOfType<BallMovement>().transform;
        rb2 = GetComponent<Rigidbody>();

        NewPos();
    }

    private void Update()
    {
        if (!isChasingPlayer)
        {
            if (Vector3.Distance(player.position, transform.position) < playerCheckRadius)
            {
                isChasingPlayer = true;
            }
        }
    }

    void FixedUpdate()
    {
        //Man ska inte alltid kunna röra på sig - Max
        if (!isMoving) return;

        if (isChasingPlayer)
        {
            ChasePlayer();
        }
        else
        {
            MoveTowardsTarget();
        }
    }

    void ChasePlayer()
    {

    }

    void MoveTowardsTarget()
    {
        Vector3 target = currentTarget;

        //Om den är tillräckligt nära target så ska fienden stanna och efter ett tag få en ny target - Max
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.x, 0, target.z)) < targetDistance)
        {
            isMoving = false;
            Invoke(nameof(NewPos), waitTimeToNewTarget);
            return;
        }

        //Kollar mot target - Max
        var lookPos = target - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        //Rör sig mot target - Max
        Vector3 newVel = transform.forward * wanderingSpeed;
        rb2.velocity = new Vector3(newVel.x, rb2.velocity.y, newVel.z);
    }

    void NewPos()
    {
        bool foundPos = false;
        while (!foundPos)
        {
            Vector3 newTarget = wanderingAreaCenter +
            new Vector3
                (
                Random.Range(-wanderingAreaSize, wanderingAreaSize),
                0,
                Random.Range(-wanderingAreaSize, wanderingAreaSize)
                );

            if (Physics.Raycast(newTarget + new Vector3(0,5,0), Vector3.down, 7, LayerMask.GetMask("Water")))
            {
                foundPos = true;

                currentTarget = newTarget;
                isMoving = true;
            }
        }       
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.transform.GetComponent<BallMovement>())
        {
            NewPos();
        }
    }

    public override void Die(Vector3 contactPoint)
    {
        //Gör så att den kan påverkas av forces - Max
        isMoving = false;
        rb2.isKinematic = false;
        rb2.constraints = RigidbodyConstraints.None;
        
        base.Die(contactPoint);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,255,0,0.4f);

        Gizmos.DrawSphere(transform.position, playerCheckRadius);

        if (Application.isPlaying)
        {
            Gizmos.DrawCube(wanderingAreaCenter, new Vector3(wanderingAreaSize * 2, wanderingAreaSize * 2, wanderingAreaSize * 2));
        }
        else
        {
            Gizmos.DrawCube(transform.position, new Vector3(wanderingAreaSize * 2, wanderingAreaSize * 2, wanderingAreaSize * 2));
        }
    }
}
