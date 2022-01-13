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
        //Kolla spelarens script och ta lastVelocity för att se om velocity > speedToDIe, då ska de dö.

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
        //Så den inte dör flera gånger
        hasDied = true;

        //Använder formeln angle = point1 - point2 för att ta fram vinkeln mellan spelaren och fienden
        Vector3 dir = contactPoint - transform.position;

        //Reversar vinkeln så den går bort från spelaren istället och normalizar den så att jag bara får vinkeln av vektorn
        dir = -dir.normalized;

        //Lägger på en force i direction
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
