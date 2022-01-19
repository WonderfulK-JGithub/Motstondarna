using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class LaserScript : MonoBehaviour //Skriptet �r till f�r att f� lasern att se lite b�ttre ut - Max
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
