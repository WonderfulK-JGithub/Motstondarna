using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class WanderingEnemy : MonoBehaviour
{
    [SerializeField] Vector3 wanderingAreaCenter;

    [SerializeField] float wanderingAreaRadius;

    [SerializeField] float wanderingSpeed;
    [SerializeField] float chasingSpeed;

    [SerializeField] float rotationSpeed;

    [SerializeField] float targetDistance;

    [SerializeField] float waitTimeToNewTarget;

    [SerializeField] Vector3 currentTarget;

    Transform player;

    bool isChasingPlayer = false;

    bool isMoving = false;

    Rigidbody rb;

    void Start()
    {
        wanderingAreaCenter = transform.position;
        player = FindObjectOfType<BallMovement>().transform;
        rb = GetComponent<Rigidbody>();

        NewPos();
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        if (!isMoving) return;

        Vector3 target = isChasingPlayer ? player.position : currentTarget;

        if (!GroundCheck() || Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.x, 0, target.z)) < targetDistance)
        {
            isMoving = false;
            Invoke(nameof(NewPos), waitTimeToNewTarget);
            return;
        }

        float speed = isChasingPlayer ? chasingSpeed : wanderingSpeed;

        //Kollar mot target
        var lookPos = target - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        //Rör sig mot target
        Vector3 newVel = transform.forward * speed;
        rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
    }

    bool GroundCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + transform.forward * 1, Vector3.down, Color.red, 4);

        if (Physics.Raycast(transform.position + transform.forward * 1, Vector3.down, out hit, 4, LayerMask.GetMask("Water")))
        {
            Debug.DrawRay(transform.position + transform.forward * 1, Vector3.down, Color.red, 4);
            return true;
        }
        else
        {
            return false;
        }
    }

    void NewPos()
    {
        Vector3 newTarget = wanderingAreaCenter + 
            new Vector3
                (
                Random.Range(-wanderingAreaRadius, wanderingAreaRadius), 
                0,
                Random.Range(-wanderingAreaRadius, wanderingAreaRadius)
                );

        currentTarget = newTarget;
        isMoving = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,255,0,0.4f);


        if (Application.isPlaying)
        {
            Gizmos.DrawSphere(wanderingAreaCenter, wanderingAreaRadius);
        }
        else
        {
            Gizmos.DrawSphere(transform.position, wanderingAreaRadius);

        }
    }
}
