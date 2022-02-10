using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour
{
    [SerializeField] GameObject waterSplashPS;

    private void OnTriggerEnter(Collider other)
    {
        SoundManagerScript.PlaySound("WaterSplash");
        Destroy(Instantiate(waterSplashPS, other.transform.position, Quaternion.identity), 5f);

    }
}
