using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    [Header("X-Z Movement")]
    [SerializeField] float topSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float extraAccelerationFactor;
    [SerializeField] Transform orientationTransform;
    [SerializeField] Slider speedBar;
    [SerializeField] Image fillImage;
    [SerializeField] Gradient speedColors;

    float inputX;
    float inputZ;

    [Header("Jumping")]
    [SerializeField] float jumpStrength;
    [SerializeField] float jumpHoldTime;
    [SerializeField] float extraGravityFactor;
    [SerializeField] float terminalVelocity;
    [SerializeField] LayerMask groundLayers;
    [SerializeField] LayerMask slipparyLayer;

    float holdTimer;

    float xAcceleration;
    float zAcceleration;
    Vector3 targetSpeed;

    [Header("Dashing")]
    [SerializeField] float dashTime;
    [SerializeField] GameObject dashTrail;

    float dashTimer;

    [Header("Other")]
    [SerializeField] ParticleSystem chargeParticle;
    [SerializeField] float slideExtraGravity;

    bool onGround;
    bool onSlippary;
    bool isJumping;
    bool canDash;

    bool noInput;

    [HideInInspector] public Vector3 currentSpeed;
    Vector3 accelerationDirection;

    Rigidbody rb;
    PlayerState state;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }


    public virtual void Update()
    {
        
        switch (state)
        {
            case PlayerState.Free:
                #region

                //h�mtar inputs f�r vilket h�ll man ska g�
                inputX = Input.GetAxisRaw("Horizontal");
                inputZ = Input.GetAxisRaw("Vertical");

                noInput = inputX == 0 && inputZ == 0;//variabel som kollar om man tryckt �t n�got h�ll alls

                
                //Den speed som bollen ska accelerera mot
                targetSpeed = new Vector3(inputX * topSpeed * orientationTransform.right.x + inputZ * topSpeed * orientationTransform.forward.x, 0f, inputX * topSpeed * orientationTransform.right.z + inputZ * topSpeed * orientationTransform.forward.z);

                

                
                
                


                



                //Hopp
                if (Input.GetButtonDown("Jump") && onGround)
                {
                    rb.velocity = new Vector3(rb.velocity.x, jumpStrength, rb.velocity.z);//ger en y velocity
                    isJumping = true;
                    holdTimer = jumpHoldTime;
                }

                //Om man sl�pper hoppknappen, eller att jumptimern g�r ut, blir isJumping false
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

                //Dash
                if(Input.GetButtonDown("Fire1") && canDash)
                {
                    state = PlayerState.ChargeDash;//�ndrar state
                    chargeParticle.Play();//S�tter ig�ng particles
                    canDash = false;//g�r att man inte kan dasha (f�r�ns variabeln blir true igen)

                    rb.useGravity = false;//st�nger av gravitation

                    dashTrail.transform.position = transform.position;
                    dashTrail.SetActive(false);
                }

                #endregion
                break;
            case PlayerState.ChargeDash:
                #region

                currentSpeed = Vector3.Lerp(currentSpeed, Vector3.zero, 0.05f);//�ndrar bollens hastighet l�ngsamt till 0

                //kollar om man sl�pper dashKnappen
                if(!Input.GetButton("Fire1"))
                {
                    chargeParticle.Stop();//St�nger av particle systemet
                    state = PlayerState.Dash;//�ndrar state

                    rb.velocity = orientationTransform.forward * topSpeed;//�ndrar hastigheten
                    currentSpeed = rb.velocity;
                    dashTimer = dashTime;

                    accelerationDirection = currentSpeed / topSpeed;

                    dashTrail.SetActive(true);
                }
                #endregion
                break;
            case PlayerState.Dash:
                #region
                dashTimer -= Time.deltaTime;

                if(dashTimer <= 0f)
                {
                    state = PlayerState.Free;

                    

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
                if (!noInput)
                {
                    accelerationDirection = targetSpeed / topSpeed;//variabel som best�mmer acceleration baserat p� riktingen bollen ska �ka

                    //r�knar ut vilken acceleration man ska ha i x och z led. Tanken med detta �r att man ska ha starkare acceleration n�r man tv�rsv�nger
                    xAcceleration = Mathf.Sign(targetSpeed.x) == Mathf.Sign(currentSpeed.x) ? acceleration : acceleration * extraAccelerationFactor;
                    zAcceleration = Mathf.Sign(targetSpeed.z) == Mathf.Sign(currentSpeed.z) ? acceleration : acceleration * extraAccelerationFactor;
                }
                else
                {
                    xAcceleration = acceleration;
                    zAcceleration = acceleration;
                }


                xAcceleration *= Mathf.Abs(accelerationDirection.x);
                zAcceleration *= Mathf.Abs(accelerationDirection.z);


                if ((onGround && !onSlippary) || !noInput)//�r man i luften eller p� halt golv OCH inte trycker �t n�got h�ll beh�ller man den hastighet man hade
                {
                    currentSpeed.x = Mathf.MoveTowards(rb.velocity.x, targetSpeed.x, xAcceleration * Time.fixedDeltaTime);
                    currentSpeed.z = Mathf.MoveTowards(rb.velocity.z, targetSpeed.z, zAcceleration * Time.fixedDeltaTime);
                }

                rb.velocity = new Vector3(currentSpeed.x, rb.velocity.y, currentSpeed.z);

                if (Physics.Raycast(transform.position, Vector3.down, 0.52f, groundLayers))
                {
                    onGround = true;
                    canDash = true;
                }
                else
                {
                    onGround = false;
                }
                if (Physics.Raycast(transform.position, Vector3.down, 0.52f, slipparyLayer))
                {
                    onSlippary = true;
                }
                else
                {
                    onSlippary = false;
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
            case PlayerState.Dash:
                dashTrail.transform.position = transform.position;
                break;
        }

        rb.angularVelocity = new Vector3(rb.velocity.z,0f,rb.velocity.x);

        float _value = new Vector3(rb.velocity.x,0f,rb.velocity.z).magnitude / (topSpeed * 0.95f);

        speedBar.value = Mathf.Lerp(speedBar.value,_value,0.3f);

        fillImage.color = speedColors.Evaluate(_value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("R�nna") && state == PlayerState.Free)
        {
            state = PlayerState.Renn;
            currentSpeed = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("R�nna"))
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