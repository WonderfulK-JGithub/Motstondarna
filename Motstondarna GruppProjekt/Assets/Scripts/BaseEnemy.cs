using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class BaseEnemy : MonoBehaviour
{
    [SerializeField] float playerVelocityForDeath;
    [SerializeField] float dieForce;
    [SerializeField] float fadeSpeed;

    [SerializeField] bool canDieFromOtherPins = false;
    [SerializeField] bool canKillPlayer = false;

    public bool hasDied = false;
    //bool hasDiedFromPlayer;
    //Referenser
    MeshRenderer rend;
    [HideInInspector] public Rigidbody rb;

    [SerializeField] GameObject deathParticle;

    public virtual void Awake()
    {
        rend = GetComponentInChildren<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        OnAnyCollisionEnter(collision.collider, collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(gameObject.layer == 9)
            OnAnyCollisionEnter(other);
    }

    void OnAnyCollisionEnter(Collider other, Collision collision = null)
    {
        if ((other.transform.GetComponent<BallMovement>() && other.transform.GetComponent<BallMovement>().currentSpeed.magnitude >= playerVelocityForDeath))
        {
            if ( true)
            {
                Die(collision != null ? collision.GetContact(0).point : other.transform.position, other.transform.GetComponent<BallMovement>().currentSpeed);
            }
        }
        else if (other.transform.GetComponent<BallMovement>() && other.transform.GetComponent<BallMovement>().currentSpeed.magnitude < playerVelocityForDeath)
        {
            rb.isKinematic = true;

            //Använder formeln angle = point1 - point2 för att ta fram vinkeln mellan spelaren och fienden - Max
            Vector3 dir = collision != null ? collision.GetContact(0).point : other.transform.position - transform.position;

            //Normalizar vinkeln den så att jag bara får vinkeln av vektorn - Max
            dir = dir.normalized;

            //Lägger på en force i direction - Max
            other.transform.GetComponent<BallMovement>().currentSpeed = dir * 5;


            if(canKillPlayer)
                other.transform.GetComponent<BallHealth>().TakeDamage(Vector3.zero, 1);
        }
        else if (canDieFromOtherPins && other.transform.GetComponent<BaseEnemy>())
        {
            Vector3 dir = collision.GetContact(0).point - transform.position;

            //Normalizar vinkeln den så att jag bara får vinkeln av vektorn - Max
            dir = -dir.normalized;

            Die(collision.GetContact(0).point, /*dir * dieForce*/ Vector3.zero);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.GetComponent<BallMovement>())
            rb.isKinematic = false;
    }

    public virtual void Die(Vector3 contactPoint, Vector3 speed)
    {
        SoundManagerScript.PlaySound("KägglaDamage");
        SpawnParticles();

        //Så den inte dör flera gånger - Max
        hasDied = true;

        //Använder formeln angle = point1 - point2 för att ta fram vinkeln mellan spelaren och fienden - Max
        Vector3 dir = contactPoint - transform.position;

        //Reversar vinkeln så den går bort från spelaren istället och normalizar den så att jag bara får vinkeln av vektorn - Max
        //dir = -dir.normalized;
        dir = speed.normalized;

        //Lägger på en force i direction - Max
        //GetComponent<Rigidbody>().AddForce(dir * dieForce);
        GetComponent<Rigidbody>().AddForceAtPosition(dir * dieForce, contactPoint);

        StartCoroutine(Fade());
    }

    public void Die()
    {
        SoundManagerScript.PlaySound("KägglaDamage");
        SpawnParticles();
        hasDied = true;
        StartCoroutine(Fade());
    }

    public void DieNow()
    {
        SoundManagerScript.PlaySound("KägglaDamage");
        SpawnParticles();
        hasDied = true;
        Destroy(gameObject);
    }

    void SpawnParticles()
    {
        GameObject newParticle = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(newParticle, 1);
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(2);

        Color color;
        while (rend.material.color.a > 0)
        {
            color = rend.material.color;
            rend.material.color = new Color(color.r, color.g, color.b, color.a - (Time.deltaTime * fadeSpeed));
            yield return null;
        }

        Destroy(gameObject);
    }
}
