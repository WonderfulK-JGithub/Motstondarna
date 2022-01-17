using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class LaserEnemy : MonoBehaviour
{
    //De lasrarna som �r ig�ng - Max
    [SerializeField] GameObject[] activeLasers = new GameObject[2];

    [Header("Parameters")]

    [SerializeField] float laserMaxDistance;

    [SerializeField] float laserStartRotation;

    [SerializeField] float laserRotationSpeed;

    bool lasersOn = false;

    [Header("References")]

    [SerializeField] GameObject laserObject;

    //Lasrarna ska komma ut ur �gonen - Max
    [SerializeField] Transform[] eyes = new Transform[2];

    //F�r att inte beh�va g�ra en raycast per �gon s� finns det en annan transform som anv�nds f�r raycast origin - Max
    [SerializeField] Transform laserOrigin; //�r ocks� parent till eyes

    private void Update()
    {
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

            laserOrigin.rotation = Quaternion.Lerp(laserOrigin.rotation, Quaternion.Euler(0, 0, 0), laserRotationSpeed * Time.deltaTime);
        }
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
