using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class LaserScript : MonoBehaviour //Skriptet är till för att få lasern att se lite bättre ut - Max
{
    [SerializeField] Gradient colorGradient;
    [SerializeField] float speed;

    MeshRenderer rend;

    float _gradientTime;
    float GradientTime
    {
        get { return _gradientTime; }
        set
        { 
            if(value <= 1)
            {
                _gradientTime = value;
            }
            else
            {
                _gradientTime = 0;
            }
        }
    }

    private void Awake()
    {
        rend = GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        GradientTime += Time.deltaTime * speed;

        rend.material.color = colorGradient.Evaluate(GradientTime);
    }
}
