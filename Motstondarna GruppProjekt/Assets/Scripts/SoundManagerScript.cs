using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip AmbientWind, Checkpoint, Click, Dash, JumpPad, K‰gglaDamage, Landa, PowerUp, Rolling, R‰nna, Skada, Laser÷gon, RocketFiende; //Definerar audiclippen /Theo
    static AudioSource audioSrc;

    void Start() //Parar ihop r‰tt variebel med motsvarande ljudfil
    {
        AmbientWind = Resources.Load<AudioClip>("AmbientWind");
        Checkpoint = Resources.Load<AudioClip>("Checkoint");
        Click = Resources.Load<AudioClip>("Click");
        Dash = Resources.Load<AudioClip>("Dash");
        JumpPad = Resources.Load<AudioClip>("JumpPad");
        K‰gglaDamage = Resources.Load<AudioClip>("K‰gglaDamage");
        Landa = Resources.Load<AudioClip>("Landa");
        PowerUp = Resources.Load<AudioClip>("PowerUp");
        Rolling = Resources.Load<AudioClip>("Rolling");
        R‰nna = Resources.Load<AudioClip>("R‰nna");
        Skada = Resources.Load<AudioClip>("Skada");
        Laser÷gon = Resources.Load<AudioClip>("Laser÷gon");
        RocketFiende = Resources.Load<AudioClip>("RocketFiende");

        audioSrc = GetComponent<AudioSource>();
    }
    void Update()
    {

    }

    public static void PlaySound(string clip) //Anv‰nder clip string value som en paramiter d‰r den parar ihop "AmbientWind" med korrekt ljudfil /Theo
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
            case "K‰gglaDamage":
                audioSrc.PlayOneShot(K‰gglaDamage);
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
            case "R‰nna":
                audioSrc.PlayOneShot(R‰nna);
                break;
            case "Skada":
                audioSrc.PlayOneShot(Skada);
                break;
            case "Laser÷gon":
                audioSrc.PlayOneShot(Laser÷gon);
                break;
            case "RocketFiende":
                audioSrc.PlayOneShot(RocketFiende);
                break;
        }
    }
}

//Fˆr att spela ljuden SoundManagerScript.PlaySound ("InsertName"); /Theo