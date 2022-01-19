using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class WanderingEnemy : BaseEnemy
{
    [Header("Wandering Area")]
    [SerializeField] Vector3 wanderingAreaCenter; //Runt den här punkten så wanderar den - Max
    [SerializeField] float wanderingAreaSize; //Hur stor area som den wanderar - Max

    [Header("Wandering parameters")]
    [SerializeField] float wanderingSpeed; //Hur snabbt den går - Max
    [SerializeField] float targetDistance; //Hur nära den behöver vara till target för att det ska räknas som att den är framme - Max
    [SerializeField] float waitTimeToNewTarget; //Hur länge den ska stå still tills den tar en ny target att gå till - Max
    [SerializeField] Vector3 currentTarget; //Dit den är på väg just nu - Max

    [Header("Checking For Player")]
    [SerializeField] float playerCheckRadius; //Hur stor radius som den kollar efter spelaren i - max

    [Header("Chasing parameters")]
    [SerializeField] float chasingSpeed; //Hur snabbt den jagar spelaren - max
    [SerializeField] float rotationSpeed; //Hur snabbt den roterar när den rör sig - max

    bool isChasingPlayer = false; //Jagar spelaren - Max
    bool isMoving = false; //Rör på sig - Max
    bool canCheckForPlayer = true; 
    [HideInInspector] public bool overrideChasing = false; //är public så att andra skript kan accessa den - Max

    //References
    Transform player;
    Animator anim;

    public override void Awake()
    {
        base.Awake();

        player = FindObjectOfType<BallMovement>().transform;
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        wanderingAreaCenter = transform.position; //Den ska wandera runt där den först placeras - max

        NewPos(); //Hittar en ny target pos att gå till - Max
    }

    private void Update()
    {
        if (hasDied) return;

        if (canCheckForPlayer && !isChasingPlayer)
        {
            if (Vector3.Distance(player.position, transform.position) < playerCheckRadius)
            {
                StartChasing();
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
        //Man ska inte alltid kunna röra på sig - Max
        if (!isMoving || hasDied) return;

        if (isChasingPlayer)
        {
            if (!overrideChasing)
            {
                ChasePlayer();
            }
        }
        else
        {
            Wandering();
        }
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
            rb.velocity = Vector3.zero;
            canCheckForPlayer = false;
        }
    }

    void Wandering()
    {
        Vector3 target = currentTarget;

        //Om den är tillräckligt nära target så ska fienden stanna och efter ett tag få en ny target - Max
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.x, 0, target.z)) < targetDistance)
        {
            isMoving = false;

            if (anim != null)
                anim.speed = 0;

            Invoke(nameof(NewPos), waitTimeToNewTarget);
            return;
        }

        MoveTowardsTarget(target, wanderingSpeed);
    }

    void StopChasing()
    {
        isChasingPlayer = false;
    }

    public void StartChasing()
    {
        if(anim != null)
            anim.speed = 1;

        isChasingPlayer = true;
    }

    void MoveTowardsTarget(Vector3 target, float speed)
    {
        //Kollar mot target - Max
        var lookPos = target - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        //Rör sig mot target - Max
        Vector3 newVel = transform.forward * speed;
        rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
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

        if(anim != null)
            anim.speed = 1;
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
        //Gör så att den kan påverkas av forces - Max
        isMoving = false;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        
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
