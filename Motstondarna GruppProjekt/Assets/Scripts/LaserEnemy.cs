using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class LaserEnemy : MonoBehaviour
{
    //De lasrarna som är igång - Max
    [SerializeField] GameObject[] activeLasers = new GameObject[2];

    [SerializeField] float laserRotateSpeed;

    [Header("Parameters")]

    [SerializeField] float laserMaxDistance;

    [SerializeField] float laserStartRotation;

    [SerializeField] float laserActivationRotationSpeed;

    [SerializeField] float laserAttackActivateRadius;

    [SerializeField] LayerMask laserMask;

    public bool lasersOn = false;

    [Header("References")]

    [SerializeField] GameObject laserObject;

    //Lasrarna ska komma ut ur ögonen - Max
    [SerializeField] Transform[] eyes = new Transform[2];

    //För att inte behöva göra en raycast per ögon så finns det en annan transform som används för raycast origin - Max
    [SerializeField] Transform laserOrigin; //Är också parent till eyes

    WanderingEnemy wanderingScript;
    [SerializeField] Transform player;

    private void Awake()
    {
        wanderingScript = GetComponent<WanderingEnemy>();
    }

    private void Update()
    {
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

                if (Physics.Raycast(eyes[i].position, eyes[i].forward, out hit, laserMaxDistance, laserMask))
                {
                    if(hit.collider.gameObject.tag == "Player")
                    {
                        //Damage
                        Debug.Log("Hit player");
                    }

                    UpdateLaserScale(hit.distance, i);
                }
                else
                {
                    UpdateLaserScale(laserMaxDistance, i);
                }
            }            

            laserOrigin.localEulerAngles = new Vector3(Mathf.Lerp(laserOrigin.eulerAngles.x, 0, laserActivationRotationSpeed * Time.deltaTime), 0, 0);
        }

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

        //Attempt 2
        //Quaternion limitedRotation = new Quaternion(0f, 0f, 0f, 0f);
        //Quaternion rotation = Quaternion.LookRotation(player.position - transform.position);
        ////lock rotation, yaw only
        //rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);

        ////rotates the Ship to Reticle with fixed speed
        //limitedRotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateAcceleration * Time.deltaTime);

        //transform.rotation = limitedRotation;

        //Attempt 1
        //    var lookPos = player.position - transform.position;
        //    lookPos.y = 0;
        //    var rotation = Quaternion.LookRotation(lookPos);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateAcceleration);
    }

    void UpdateLaserScale(float length, int id)
    {
        activeLasers[id].transform.localScale = new Vector3(1, 1, length) * (1 / 0.75f);
    }

    void UpdateLaserScale(float length)
    {
        for (int i = 0; i <= 1; i++)
        {
            activeLasers[i].transform.localScale = new Vector3(1, 1, length) * (1 / 0.75f);
        }
    }

    public void TurnOnLasers()
    {
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
    }
}
