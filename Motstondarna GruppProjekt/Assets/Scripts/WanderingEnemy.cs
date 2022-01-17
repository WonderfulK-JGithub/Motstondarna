using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class WanderingEnemy : BaseEnemy
{
    [Header("Wandering Area")]
    [SerializeField] Vector3 wanderingAreaCenter;
    [SerializeField] float wanderingAreaSize;

    [Header("Wandering parameters")]
    [SerializeField] float wanderingSpeed;
    [SerializeField] float targetDistance;
    [SerializeField] float waitTimeToNewTarget;
    [SerializeField] Vector3 currentTarget;

    [Header("Checking For Player")]
    [SerializeField] float playerCheckRadius;

    [Header("Chasing parameters")]
    [SerializeField] float chasingSpeed;
    [SerializeField] float rotationSpeed;

    Transform player;

    bool isChasingPlayer = false;
    bool isMoving = false;
    bool canCheckForPlayer = true;

    public bool overrideChasing = false;

    //Components
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
        if (hasDied) return;

        if (canCheckForPlayer && !isChasingPlayer)
        {
            if (Vector3.Distance(player.position, transform.position) < playerCheckRadius)
            {
                isChasingPlayer = true;
            }
        }

        if (!canCheckForPlayer)
        {
            if(Vector3.Distance(player.position, transform.position) > playerCheckRadius + 4f)
            {
                canCheckForPlayer = true;
            }
        }
    }

    void FixedUpdate()
    {
        //Man ska inte alltid kunna r�ra p� sig - Max
        if (!isMoving) return;
        if (hasDied) return;

        if (isChasingPlayer)
        {
            if (!overrideChasing)
            {
                ChasePlayer();
            }
            else
            {
                AttackEveryFrame();
            }
        }
        else
        {
            Wandering();
        }
    }

    void AttackEveryFrame()
    {

    }

    void ChasePlayer()
    {
        if (GroundCheck())
        {
            MoveTowardsTarget(player.position, chasingSpeed);
        }
        else
        {
            Invoke(nameof(StopChasing), 1f);
            rb2.velocity = Vector3.zero;
            canCheckForPlayer = false;
        }
    }

    void Wandering()
    {
        Vector3 target = currentTarget;

        //Om den �r tillr�ckligt n�ra target s� ska fienden stanna och efter ett tag f� en ny target - Max
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.x, 0, target.z)) < targetDistance)
        {
            isMoving = false;
            Invoke(nameof(NewPos), waitTimeToNewTarget);
            return;
        }

        MoveTowardsTarget(target, wanderingSpeed);
    }

    void StopChasing()
    {
        isChasingPlayer = false;
    }

    void MoveTowardsTarget(Vector3 target, float speed)
    {
        //Kollar mot target - Max
        var lookPos = target - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        //R�r sig mot target - Max
        Vector3 newVel = transform.forward * speed;
        rb2.velocity = new Vector3(newVel.x, rb2.velocity.y, newVel.z);
    }

    bool GroundCheck()
    {
        Debug.DrawRay(transform.position + transform.forward * 1, Vector3.down, Color.red, 4);

        if (Physics.Raycast(transform.position + transform.forward * 1, Vector3.down, 4, LayerMask.GetMask("Ground", "Slippery")))
        {
            return true;
        }
        else
        {
            return false;
        }
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

            if (Physics.Raycast(newTarget + new Vector3(0,5,0), Vector3.down, 7, LayerMask.GetMask("Ground", "Slipper")))
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

    public override void Die(Vector3 contactPoint, Vector3 speed)
    {
        //G�r s� att den kan p�verkas av forces - Max
        isMoving = false;
        rb2.isKinematic = false;
        rb2.constraints = RigidbodyConstraints.None;
        
        base.Die(contactPoint, speed);
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
