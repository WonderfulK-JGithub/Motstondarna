using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] float speedToDie;

    [SerializeField] float deathForce;

    [SerializeField] float fadeSpeed;

    bool hasDied = false;

    MeshRenderer rend;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Kolla spelarens script och ta lastVelocity f�r att se om velocity > speedToDIe, d� ska de d�.

        if (true)
        {
            if (!hasDied)
            {
                Die(collision.GetContact(0).point);
            }
        }
    }

    void Die(Vector3 contactPoint)
    {
        //S� den inte d�r flera g�nger
        hasDied = true;

        //Anv�nder formeln angle = point1 - point2 f�r att ta fram vinkeln mellan spelaren och fienden
        Vector3 dir = contactPoint - transform.position;

        //Reversar vinkeln s� den g�r bort fr�n spelaren ist�llet och normalizar den s� att jag bara f�r vinkeln av vektorn
        dir = -dir.normalized;

        //L�gger p� en force i direction
        GetComponent<Rigidbody>().AddForce(dir * deathForce);

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
