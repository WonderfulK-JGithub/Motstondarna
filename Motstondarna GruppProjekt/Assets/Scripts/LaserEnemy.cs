using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class LaserEnemy : MonoBehaviour
{
    //De lasrarna som �r ig�ng - Max
    [SerializeField] GameObject[] activeLasers = new GameObject[2];

    [SerializeField] float rotateVelocity;
    [SerializeField] float rotateAcceleration;

    [Header("Parameters")]

    [SerializeField] float laserMaxDistance;

    [SerializeField] float laserStartRotation;

    [SerializeField] float laserActivationRotationSpeed;

    [SerializeField] float laserAttackActivateRadius;

    public bool lasersOn = false;

    [Header("References")]

    [SerializeField] GameObject laserObject;

    //Lasrarna ska komma ut ur �gonen - Max
    [SerializeField] Transform[] eyes = new Transform[2];

    //F�r att inte beh�va g�ra en raycast per �gon s� finns det en annan transform som anv�nds f�r raycast origin - Max
    [SerializeField] Transform laserOrigin; //�r ocks� parent till eyes

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
            RaycastHit hit;

            if (Physics.Raycast(laserOrigin.position, laserOrigin.forward, out hit, laserMaxDistance, LayerMask.GetMask("Water")))
            {
                UpdateLaserScale(hit.distance);
            }
            else
            {
                UpdateLaserScale(laserMaxDistance);
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
        var lookPos = player.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateAcceleration);
    }

    void UpdateLaserScale(float length)
    {
        for (int i = 0; i <= 1; i++)
        {
            activeLasers[i].transform.localScale = new Vector3(1, 1, length);
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
