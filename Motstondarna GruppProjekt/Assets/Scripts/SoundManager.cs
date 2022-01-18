using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip AmbientWind, Checkpoint, Click, Dash, JumpPad, KägglaDamage, Landa, PowerUp, Rolling, Ränna, Skada; //Definerar som audiclip /Theo
    static AudioSource audioSrc;

    void Start() //Parar ihop rätt variabel med rätt audioclip
    {
        AmbientWind = Resources.Load<AudioClip>("AmbientWind");
        Checkpoint = Resources.Load<AudioClip>("Checkoint");
        Click = Resources.Load<AudioClip>("Click");
        Dash = Resources.Load<AudioClip>("Dash");
        JumpPad = Resources.Load<AudioClip>("JumpPad");
        KägglaDamage = Resources.Load<AudioClip>("KägglaDamage");
        Landa = Resources.Load<AudioClip>("Landa");
        PowerUp = Resources.Load<AudioClip>("PowerUp");
        Rolling = Resources.Load<AudioClip>("Rolling");
        Ränna = Resources.Load<AudioClip>("Ränna");
        Skada = Resources.Load<AudioClip>("Skada");

        audioSrc = GetComponent<AudioSource>();
    }
    void Update()
    {

    }

    public static void PlaySound(string clip) //Använder clip string value som en paramiter där den parar ihop "Damage" med korrekt ljudfil /Theo
    {
        switch (clip)
        {
            case "AmbientWind":
                audioSrc.PlayOneShot(AmbientWind);
                break;
            case "Checkpoint":
                audioSrc.PlayOneShot(Checkpoint);
                break;
            case "Click":
                audioSrc.PlayOneShot(Click);
                break;
            case "Dash":
                audioSrc.PlayOneShot(Dash);
                break;
            case "JumpPad":
                audioSrc.PlayOneShot(JumpPad);
                break;
            case "KägglaDamage":
                audioSrc.PlayOneShot(KägglaDamage);
                break;
            case "Landa":
                audioSrc.PlayOneShot(Landa);
                break;
            case "PowerUp":
                audioSrc.PlayOneShot(PowerUp);
                break;
            case "Rolling":
                audioSrc.PlayOneShot(Rolling);
                break;
            case "Ränna":
                audioSrc.PlayOneShot(Ränna);
                break;
            case "Skada":
                audioSrc.PlayOneShot(Skada);
                break;



        }
    }
}

// För att spela ljuden SoundManagerScript.PlaySound ("InsertName"); /Theo