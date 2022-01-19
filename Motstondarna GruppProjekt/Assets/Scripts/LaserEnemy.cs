using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class LaserEnemy : MonoBehaviour
{
    [SerializeField] GameObject[] activeLasers = new GameObject[2];     //De lasrarna som �r ig�ng - Max

    [SerializeField] float laserRotateSpeed; //Hur snabbt fienden roterar n�r lasrarna �r p� - Max

    [Header("Parameters")]

    [SerializeField] float laserMaxDistance; //Hur l�ng som lasern kan vara - Max

    [SerializeField] float laserStartRotation; //S� att lasern b�rjar med att kolla ner i marken s� man inte instant d�r - Max

    [SerializeField] float laserActivationRotationSpeed; //Hur snabbt lasern roterar p� x-axeln allts� vinklar sig upp eller ner - Max

    [SerializeField] float laserAttackActivateRadius; //Radius f�r n�r lasern ska anv�ndas - Max

    [SerializeField] LayerMask laserMask; //Vad laserns ska collidea med

    public bool lasersOn = false;

    [Header("References")]

    [SerializeField] GameObject laserObject;

    //Lasrarna ska komma ut ur �gonen - Max
    [SerializeField] Transform[] eyes = new Transform[2];

    //F�r att inte beh�va g�ra en raycast per �gon s� finns det en annan transform som anv�nds f�r raycast origin - Max
    [SerializeField] Transform laserOrigin; //�r ocks� parent till eyes

    WanderingEnemy wanderingScript;
    [SerializeField] Transform player;

    Animator anim;

    private void Awake()
    {
        wanderingScript = GetComponent<WanderingEnemy>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        //Har man d�tt ska lasrarna st�ngas av - Max
        if (lasersOn && wanderingScript.hasDied)
        {
            TurnOffLasers();
            return;
        }

        if (lasersOn)
        {
            for (int i = 0; i < eyes.Length; i++)
            {
                RaycastHit hit;

                //En Raycast f�r varje �ga
                if (Physics.Raycast(eyes[i].position, eyes[i].forward, out hit, laserMaxDistance, laserMask))
                {
                    if(hit.collider.gameObject.tag == "Player")
                    {
                        //G�r Damage
                        FindObjectOfType<BallHealth>().TakeDamage(eyes[i].forward * 10, 1);
                    }

                    //Laserns storlek ska �ndras s� den slutar d�r raycasten tr�ffar - Max
                    UpdateLaserScale(hit.distance, i);
                }
                else
                {
                    UpdateLaserScale(laserMaxDistance, i);
                }
            }

            //Anv�nder formeln angle = point1 - point2 f�r att ta fram riktningen fr�n fienden till spelaren - Max
            Vector3 dir = player.position - laserOrigin.position;

            //Normalizar till en riktning - Max
            dir = dir.normalized;

            //G�r om till en rotation - Max
            Quaternion dirQ = Quaternion.LookRotation(dir);

            //kollar om spelaren �r �ver fiendens �gon - Max
            bool playerAbove = player.position.y > laserOrigin.position.y;

            //Lerpar rotationen s� att den roterar lasrarna upp och ner s� att den siktar mot spelaren, annars kunde man st� under lasern n�r den gick rakt ut - Max
            laserOrigin.localEulerAngles = new Vector3(Mathf.Clamp(Mathf.Lerp(laserOrigin.eulerAngles.x, playerAbove ? 0 : dirQ.eulerAngles.x, laserActivationRotationSpeed * Time.deltaTime), 1, 72), 0, 0);

            //laserOrigin.localEulerAngles = new Vector3(Mathf.Lerp(laserOrigin.eulerAngles.x, 0, laserActivationRotationSpeed * Time.deltaTime), 0, 0);
        }

        //Kollar distance till spelaren och st�nger av eller s�tter p� lasrar - Max
        if (!wanderingScript.overrideChasing && Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.position.x, 0, player.position.z)) < laserAttackActivateRadius)
        {
            wanderingScript.overrideChasing = true;
            TurnOnLasers();
        }
        else if (wanderingScript.overrideChasing && Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.position.x, 0, player.position.z)) > laserAttackActivateRadius + 4)
        {
            wanderingScript.overrideChasing = false;
            TurnOffLasers();
        }

        if (lasersOn)
        {
            RotateTowardsPlayer();
        }
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
        transform.rotation = Quaternion.RotateTowards(rotation, lookRotation, laserRotateSpeed * Time.deltaTime);
    }

    void UpdateLaserScale(float length, int id)
    {
        activeLasers[id].transform.localScale = new Vector3(1, 1, length) * (1 / 0.75f);
    }

    //F�r att uppdatera scale f�r b�da lasrarna samtidigt
    void UpdateLaserScale(float length)
    {
        for (int i = 0; i <= 1; i++)
        {
            activeLasers[i].transform.localScale = new Vector3(1, 1, length) * (1 / 0.75f);
        }
    }

    public void TurnOnLasers()
    {
        wanderingScript.StartChasing();
        anim.Play("LaserAlerted");
        SoundManagerScript.PlaySound("Laser�gon");

        StartCoroutine(nameof(tilLasersOn));
    }

    IEnumerator tilLasersOn()
    {
        yield return new WaitForSeconds(1.28333f / 2);

        lasersOn = true;

        for (int i = 0; i <= 1; i++)
        {
            activeLasers[i] = Instantiate(laserObject, eyes[i]);
        }

        laserOrigin.rotation = Quaternion.Euler(laserStartRotation, 0, 0);
    }

    public void TurnOffLasers()
    {
        lasersOn = false;

        for (int i = 1; i >= 0; i--)
        {
            Destroy(activeLasers[i]);
        }

        anim.Play("Walking");
    }
}
