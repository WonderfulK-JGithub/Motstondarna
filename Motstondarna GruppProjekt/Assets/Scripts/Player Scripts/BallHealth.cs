using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallHealth : BallMovement // av K-J
{
    public static BallHealth current;

    [Header("Health")]
    [SerializeField] int maxHealth;
    [SerializeField] float invinceTime;//hur länge man är odödlig efter att man blivit skadad av en käggla

    [SerializeField] Image healthImage;
    [SerializeField] Sprite[] healthSprites;

    public Color invinceColor;//vilken färg man har när man är odödlig (ändras av en animation som bollen har)
    Color defaultColor;

    int healthPoints;

    float invinceTimer;

    bool invinceable;

    public MeshRenderer rend;


    public override void Awake()
    {
        base.Awake();

        current = this;

        healthPoints = maxHealth;
        //rend = GetComponent<MeshRenderer>();
        defaultColor = rend.material.color;

        NewHealth();

        
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
            rend.material.color = defaultColor;
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(Vector3.zero, 1);
        }
    }

    public void TakeDamage(Vector3 knockBack,int damage)//tar bort hp och ändrar hastigheten/ "ge en knockback"
    { 
        if (invinceable) return;

        CameraController.current.ScreenShake();

        currentSpeed = knockBack;
        if(knockBack != Vector3.zero)rb.velocity = new Vector3(currentSpeed.x, rb.velocity.y, currentSpeed.z);

        healthPoints -= damage;
        NewHealth();

        if(healthPoints <= 0)
        {
            GameOver();
            invinceable = true;
            this.enabled = false;
        }
        else
        {
            invinceTimer = invinceTime;
            invinceable = true;
        }
    }

    void NewHealth()
    {
        healthPoints = Mathf.Clamp(healthPoints, 0, maxHealth);

        healthImage.sprite = healthSprites[healthPoints];

    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneTransition.current.EnterScene(3);
        SoundManagerScript.PlaySound("Game Over");

        
        Pause.gamePaused = true;
        FindObjectOfType<Pause>().enabled = false;
    }

    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.CompareTag("Heart"))
        {
            healthPoints = maxHealth;
            NewHealth();

            Destroy(other.gameObject);

            SoundManagerScript.PlaySound("PowerUp");

            PlayerPrefs.SetInt("score", score);

            
        }

    }
}
