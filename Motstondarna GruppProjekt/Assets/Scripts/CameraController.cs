using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    

    [SerializeField] float mouseSence = 3f;

    float rotationX;
    float rotationY;

    [SerializeField] Transform target;
    [SerializeField] Transform ballOrientation;

    [SerializeField] float distanceFromTarget = 5f;

    Vector3 currentRotation;
    Vector3 smoothVelocity = Vector3.zero;

    [SerializeField] float smoothTime;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            rotationX += mouseX * mouseSence;
            rotationY += mouseY * -mouseSence;

            rotationY = Mathf.Clamp(rotationY, 0, 80f);

            

            
        }

        

        Vector3 nextRotation = new Vector3(rotationX, rotationY);
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        ballOrientation.localEulerAngles = new Vector3(0f, rotationX, 0f);

    }

    void FixedUpdate()
    {
        transform.localEulerAngles = new Vector3(currentRotation.y, currentRotation.x, 0f);

        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}