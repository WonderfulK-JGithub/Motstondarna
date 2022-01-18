using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class RocketEnemy : MonoBehaviour
{
    bool rocketOn = false;
    bool alerted = false;

    [Header("Parameters")]
    [SerializeField] float rocketRotatingSpeed;
    [SerializeField] float rocketSpeed;
    [SerializeField] float rocketRotSpeedWhileActivating;

    [SerializeField] float rocketActivateRadius;
    [SerializeField] float rocketExplosionTime;

    [SerializeField] float rocketExplodeRadius;

    [SerializeField] float knockBackForce;

    [Header("References")]
    [SerializeField] Transform player;
    [SerializeField] ParticleSystem rocketParticles;
    [SerializeField] GameObject explosion;
    WanderingEnemy wanderingScript;
    Rigidbody rb;

    Animator anim;


    private void Awake()
    {
        wanderingScript = GetComponent<WanderingEnemy>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        rocketParticles.Stop();
    }

    private void Update()
    {
        if (!wanderingScript.overrideChasing && Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.position.x, 0, player.position.z)) < rocketActivateRadius)
        {
            StartRocket();
        }

        if (rocketOn && !wanderingScript.hasDied)
        {
            RocketInUpdate();
        }
        else if(alerted)
        {
            //Roterar fienden mot spelaren medan den gör startanimationen, annars kan den åka åt fel håll - Max
            RotateTowardsPlayer(rocketRotSpeedWhileActivating);
        }

        if (rocketOn && Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.position.x, 0, player.position.z)) < rocketExplodeRadius)
        {
            Explode();
        }
    }

    void StartRocket()
    {
        wanderingScript.StartChasing();
        wanderingScript.overrideChasing = true;

        anim.Play("RocketStart");
        alerted = true;
        Invoke(nameof(StartMovingRocket), 1.05f);
        StartCoroutine(nameof(tilExplode));
    }

    void StartMovingRocket()
    {
        rocketOn = true;
        rocketParticles.Play();
    }

    void RocketInUpdate()
    {
        RotateTowardsPlayer();

        //Rör sig framåt
        Vector3 newVel = transform.forward * rocketSpeed;
        rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
    }

    

    void RotateTowardsPlayer()
    {
        //Roterar
        Vector3 lookAt = transform.InverseTransformPoint(player.position);
        lookAt.y = 0;
        lookAt = transform.TransformPoint(lookAt);

        Quaternion rotation = transform.rotation;
        transform.LookAt(lookAt, transform.up);
        Quaternion lookRotation = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(rotation, lookRotation, rocketRotatingSpeed * Time.deltaTime);
    }

    //Den här overriden roterar med en annan speed en originalfunktionen
    void RotateTowardsPlayer(float rotSpeed)
    {
        //Roterar
        Vector3 lookAt = transform.InverseTransformPoint(player.position);
        lookAt.y = 0;
        lookAt = transform.TransformPoint(lookAt);

        Quaternion rotation = transform.rotation;
        transform.LookAt(lookAt, transform.up);
        Quaternion lookRotation = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(rotation, lookRotation, rotSpeed * Time.deltaTime);
    }

    IEnumerator tilExplode()
    {
        yield return new WaitForSeconds(rocketExplosionTime);
        Explode();
    }

    void Explode()
    {
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.position.x, 0, player.position.z)) < rocketExplodeRadius)
        {
            FindObjectOfType<BallHealth>().TakeDamage(new Vector3(0,1,0) * knockBackForce, 1);
        }

        Transform newExplosion = Instantiate(explosion, transform.position, Quaternion.identity).transform;
        Destroy(newExplosion.GetChild(0).gameObject, 0.2f);

        wanderingScript.DieNow();
    }
}
