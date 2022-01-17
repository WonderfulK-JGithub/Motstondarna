using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class BaseEnemy : MonoBehaviour
{
    [SerializeField] float playerVelocityForDeath;
    [SerializeField] float dieForce;
    [SerializeField] float fadeSpeed;

    bool hasDied = false;

    //Referenser
    MeshRenderer rend;
    Rigidbody rb;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        //Kolla spelarens script och ta lastVelocity f�r att se om velocity > speedToDIe, d� ska de d�. - Max

        if (collision.transform.GetComponent<BallMovement>() && collision.transform.GetComponent<BallMovement>().currentSpeed.magnitude >= playerVelocityForDeath)
        {
            if (!hasDied)
            {
                Die(collision.GetContact(0).point, collision.transform.GetComponent<BallMovement>().currentSpeed);
            }
        }
        else if (collision.transform.GetComponent<BallMovement>() && collision.transform.GetComponent<BallMovement>().currentSpeed.magnitude < playerVelocityForDeath)
        {
            rb.isKinematic = true;

            //Anv�nder formeln angle = point1 - point2 f�r att ta fram vinkeln mellan spelaren och fienden - Max
            Vector3 dir = collision.GetContact(0).point - transform.position;

            //Normalizar vinkeln den s� att jag bara f�r vinkeln av vektorn - Max
            dir = dir.normalized;

            //L�gger p� en force i direction - Max
            collision.transform.GetComponent<BallMovement>().currentSpeed = dir * 5;
        }    
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.GetComponent<BallMovement>())
            rb.isKinematic = false;
    }

    public virtual void Die(Vector3 contactPoint, Vector3 speed)
    {
        //S� den inte d�r flera g�nger - Max
        hasDied = true;

        //Anv�nder formeln angle = point1 - point2 f�r att ta fram vinkeln mellan spelaren och fienden - Max
        Vector3 dir = contactPoint - transform.position;

        //Reversar vinkeln s� den g�r bort fr�n spelaren ist�llet och normalizar den s� att jag bara f�r vinkeln av vektorn - Max
        //dir = -dir.normalized;
        dir = speed.normalized;

        //L�gger p� en force i direction - Max
        //GetComponent<Rigidbody>().AddForce(dir * dieForce);
        GetComponent<Rigidbody>().AddForceAtPosition(dir * dieForce, contactPoint);

        StartCoroutine(Fade());
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
