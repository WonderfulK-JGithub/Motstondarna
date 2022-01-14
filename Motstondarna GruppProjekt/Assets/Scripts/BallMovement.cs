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

    [Header("Dashing")]
    [SerializeField] float dashChargePower;
    [SerializeField] float dashMaxCharge;
    [SerializeField] float reducedAccelerationFactor;
    [SerializeField] float reducedAccelerationTime;
    [SerializeField] float dashTime;

    [Header("Other")]
    [SerializeField] ParticleSystem chargeParticle;
    [SerializeField] float slideExtraGravity;

    float inputX;
    float inputZ;

    float holdTimer;

    float totalCharge;
    float accReduction;
    float accReductionTimer;
    float dashTimer;

    bool onGround;
    bool isJumping;
    bool canDash;

    [HideInInspector] public Vector3 currentSpeed;

    Rigidbody rb;

    PlayerState state;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        Vector3 targetSpeed;
        switch (state)
        {
            case PlayerState.Free:
                #region
                inputX = Input.GetAxisRaw("Horizontal");
                inputZ = Input.GetAxisRaw("Vertical");

                bool noInput = inputX == 0 && inputZ == 0;

                

                targetSpeed = new Vector3(inputX * topSpeed * orientationTransform.right.x + inputZ * topSpeed * orientationTransform.forward.x, 0f, inputX * topSpeed * orientationTransform.right.z + inputZ * topSpeed * orientationTransform.forward.z);

                float xAcceleration;
                float zAcceleration;

                
                if(accReductionTimer > 0)
                {
                    accReductionTimer -= Time.deltaTime;
                    accReduction = reducedAccelerationFactor;
                }
                else
                {
                    accReduction = 1f;
                }

                if (!noInput)
                {
                    

                    xAcceleration = Mathf.Sign(targetSpeed.x) == Mathf.Sign(currentSpeed.x) ? acceleration : acceleration * extraAccelerationFactor * accReduction;
                    zAcceleration = Mathf.Sign(targetSpeed.z) == Mathf.Sign(currentSpeed.z) ? acceleration : acceleration * extraAccelerationFactor * accReduction;

                    //xAcceleration *= Mathf.Abs(inputX * orientationTransform.right.x + inputZ * orientationTransform.forward.x);
                    //zAcceleration *= Mathf.Abs(inputX * orientationTransform.right.z + inputZ * orientationTransform.forward.z);
                }
                else
                {
                    xAcceleration = acceleration;
                    zAcceleration = acceleration;

                    
                }

                
                
                

                if (onGround || !noInput)
                {
                    currentSpeed.x = Mathf.MoveTowards(currentSpeed.x, targetSpeed.x, xAcceleration * Time.deltaTime);
                    currentSpeed.z = Mathf.MoveTowards(currentSpeed.z, targetSpeed.z, zAcceleration * Time.deltaTime);
                }



                if (Input.GetButtonDown("Jump") && onGround)
                {
                    rb.velocity = new Vector3(rb.velocity.x, jumpStrength, rb.velocity.z);
                    isJumping = true;
                    holdTimer = jumpHoldTime;
                }


                if (isJumping && !onGround)
                {
                    if (Input.GetButton("Jump"))
                    {
                        holdTimer -= Time.deltaTime;
                        if (holdTimer <= 0)
                        {
                            isJumping = false;
                        }
                    }
                    else
                    {
                        isJumping = false;
                    }
                }

                if(Input.GetButtonDown("Fire1") && canDash)
                {
                    state = PlayerState.ChargeDash;
                    chargeParticle.Play();
                    canDash = false;
                    rb.velocity = Vector2.zero;
                    rb.useGravity = false;
                }

                #endregion
                break;
            case PlayerState.ChargeDash:
                #region
                //currentSpeed.x = Mathf.MoveTowards(currentSpeed.x, 0, acceleration * Time.deltaTime);
                //currentSpeed.z = Mathf.MoveTowards(currentSpeed.z, 0, acceleration * Time.deltaTime);

                totalCharge += dashChargePower * Time.deltaTime;

                totalCharge = Mathf.Clamp(totalCharge,0, dashMaxCharge);

                currentSpeed = Vector3.Lerp(currentSpeed, Vector3.zero, 0.05f);

                if(!Input.GetButton("Fire1"))
                {
                    chargeParticle.Stop();
                    state = PlayerState.Dash;

                    rb.velocity = orientationTransform.forward * topSpeed;
                    dashTimer = dashTime;

                }
                #endregion
                break;
            case PlayerState.Dash:
                #region
                dashTimer -= Time.deltaTime;

                if(dashTimer <= 0f)
                {
                    state = PlayerState.Free;

                    currentSpeed = rb.velocity;

                    rb.useGravity = true;
                }

                #endregion
                break;
        }

    }

    void FixedUpdate()
    {
        switch(state)
        {
            case PlayerState.Free:
                #region
                rb.velocity = new Vector3(currentSpeed.x, rb.velocity.y, currentSpeed.z);

                if (Physics.Raycast(transform.position, Vector3.down, 0.5f, groundLayers))
                {
                    onGround = true;
                    canDash = true;
                }
                else
                {
                    onGround = false;
                }


                if (!onGround && !isJumping)
                {
                    rb.velocity += new Vector3(0f, Physics.gravity.y * (extraGravityFactor - 1) * Time.fixedDeltaTime, 0f);
                }

                float yVelocityClamped = Mathf.Clamp(rb.velocity.y, terminalVelocity, 69420f);
                rb.velocity = new Vector3(rb.velocity.x, yVelocityClamped, rb.velocity.z);

                chargeParticle.transform.position = transform.position;
                #endregion
                break;
            case PlayerState.ChargeDash:

                rb.velocity = new Vector3(currentSpeed.x, currentSpeed.y, currentSpeed.z);
                chargeParticle.transform.position = transform.position;
                break;
            case PlayerState.Renn:
                rb.velocity += new Vector3(0f, Physics.gravity.y * (slideExtraGravity - 1) * Time.fixedDeltaTime, 0f);
                break;
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ränna"))
        {
            state = PlayerState.Renn;
            currentSpeed = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ränna"))
        {
            state = PlayerState.Free;
            currentSpeed = rb.velocity;
            currentSpeed.y = 0f;
        }
    }
    
}





public enum PlayerState
{
    Free,
    ChargeDash,
    Dash,
    Renn,
}