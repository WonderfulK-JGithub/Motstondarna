using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [Header("X-Z Movement")]
    [SerializeField] float topSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float extraAccelerationFactor;

    [SerializeField] Transform orientationTransform;

    [Header("Jumping")]
    [SerializeField] float jumpStrength;
    [SerializeField] float jumpHoldTime;
    [SerializeField] float extraGravityFactor;
    [SerializeField] float terminalVelocity;
    [SerializeField] LayerMask groundLayers;

    float inputX;
    float inputZ;

    float holdTimer;

    bool onGround;
    bool isJumping;

    Vector3 currentSpeed;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");

        bool noInput = inputX == 0 && inputZ == 0;

        Vector3 targetSpeed;

        targetSpeed = new Vector3(inputX * topSpeed * orientationTransform.right.x + inputZ * topSpeed * orientationTransform.forward.x, 0f, inputX * topSpeed * orientationTransform.right.z + inputZ * topSpeed * orientationTransform.forward.z);

        float xAcceleration;
        float zAcceleration;
        if (!noInput)
        {
            xAcceleration = Mathf.Sign(targetSpeed.x) == Mathf.Sign(currentSpeed.x) ? acceleration : acceleration * extraAccelerationFactor;
            zAcceleration = Mathf.Sign(targetSpeed.z) == Mathf.Sign(currentSpeed.z) ? acceleration : acceleration * extraAccelerationFactor;
        }
        else
        {
            xAcceleration = acceleration;
            zAcceleration = acceleration;
        }
        
        if(onGround || !noInput)
        {
            currentSpeed.x = Mathf.MoveTowards(currentSpeed.x, targetSpeed.x, xAcceleration * Time.deltaTime);
            currentSpeed.z = Mathf.MoveTowards(currentSpeed.z, targetSpeed.z, zAcceleration * Time.deltaTime);
        }
        


        if(Input.GetButtonDown("Jump") && onGround)
        {
            rb.velocity = new Vector3(rb.velocity.x,jumpStrength,rb.velocity.z);
            isJumping = true;
            holdTimer = jumpHoldTime;
        }


        if(isJumping && !onGround)
        {
            if(Input.GetButton("Jump"))
            {
                holdTimer -= Time.deltaTime;
                if(holdTimer <= 0)
                {
                    isJumping = false;
                }
            }
            else
            {
                isJumping = false;
            }
        }
         




    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(currentSpeed.x, rb.velocity.y, currentSpeed.z);


        if(Physics.Raycast(transform.position, Vector3.down, 0.5f, groundLayers))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }


        if(!onGround && !isJumping)
        {
            rb.velocity += new Vector3(0f, Physics.gravity.y * (extraGravityFactor - 1) * Time.fixedDeltaTime, 0f);
        }

        float yVelocityClamped = Mathf.Clamp(rb.velocity.y, terminalVelocity, 69420f);
        rb.velocity = new Vector3(rb.velocity.x, yVelocityClamped, rb.velocity.z);
    }
}