using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHealth : BallMovement
{
    [Header("Health")]
    [SerializeField] int maxHealth;
    [SerializeField] float invinceTime;//hur l�nge man �r od�dlig efter att man blivit skadad av en k�ggla

    public Color invinceColor;//vilken f�rg man har n�r man �r od�dlig (�ndras av en animation som bollen har)

    int healthPoints;

    float invinceTimer;

    bool invinceable;

    MeshRenderer rend;


    public override void Awake()
    {
        base.Awake();

        healthPoints = maxHealth;
        rend = GetComponent<MeshRenderer>();

        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(invinceable)
        {
            float _time = 1f - (invinceTimer / invinceTime);

            invinceTimer -= Time.deltaTime;
            if(invinceTimer <= 0)
            {
                invinceable = false;
            }

            rend.material.color = invinceColor;
            
        }
        else
        {
            rend.material.color = Color.white;
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(Vector3.zero, 0);
        }
    }

    public void TakeDamage(Vector3 knockBack,int damage)//tar bort hp och �ndrar hastigheten/ "ge en knockback"
    {
        
        if (invinceable) return;

        currentSpeed = knockBack;
        rb.velocity = new Vector3(currentSpeed.x, rb.velocity.y, currentSpeed.z);

        healthPoints -= damage;

        if(healthPoints <= 0)
        {
            GameOver();
        }
        else
        {
            invinceTimer = invinceTime;
            invinceable = true;
        }
    }

    public void GameOver()
    {
        
    }

    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.CompareTag("Heart"))
        {
            healthPoints++;

            Destroy(other.gameObject);
        }

    }
}
