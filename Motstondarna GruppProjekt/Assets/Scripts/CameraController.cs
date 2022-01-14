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
    [SerializeField] Transform chargeParticle;

    [SerializeField] float maxDistanceFromTarget = 5f;

    Vector3 currentRotation;
    Vector3 smoothVelocity = Vector3.zero;

    [SerializeField] float smoothTime;
    [SerializeField] float smoothSpeed;
    [SerializeField] LayerMask playerLayer;

    [SerializeField] float theGaming;


    float distanceFromTarget;

    bool firstPerson;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        distanceFromTarget = maxDistanceFromTarget;
    }
    void Update()
    {
        if(!firstPerson)
        {
            if (Input.GetMouseButton(0) || true)
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
            chargeParticle.localEulerAngles = new Vector3(0f, rotationX, 0f);

            if(Input.GetMouseButtonDown(1))
            {
                firstPerson = true;
                transform.SetParent(target);
                transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                firstPerson = false;
                transform.parent = null;
            }
        }
       

    }

    void FixedUpdate()
    {
        if(!firstPerson)
        {
            transform.localEulerAngles = new Vector3(currentRotation.y, currentRotation.x, 0f);

            if (Physics.Raycast(target.position, transform.forward * -1f, out RaycastHit hit, maxDistanceFromTarget, playerLayer))
            {
                //transform.position = hit.point;
                distanceFromTarget = Mathf.Lerp(distanceFromTarget, hit.distance, smoothSpeed) - theGaming;
            }
            else
            {
                //transform.position = target.position - transform.forward * maxDistanceFromTarget;
                distanceFromTarget = Mathf.Lerp(distanceFromTarget, maxDistanceFromTarget, smoothSpeed) - theGaming;
            }

            transform.position = target.position - transform.forward * distanceFromTarget;
        }
       
    }
}