using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip Ljud; //Definerar som audiclip /Theo
    static AudioSource audioSrc;

    void Start() //Parar ihop r�tt variabel med r�tt audioclip
    {
        Ljud = Resources.Load<AudioClip>("Ljud");

        audioSrc = GetComponent<AudioSource>();
    }
    void Update()
    {

    }

    public static void PlaySound(string clip) //Anv�nder clip string value som en paramiter d�r den parar ihop "Damage" med korrekt ljudfil /Theo
    {
        switch (clip)
        {
            case "Ljud":
                audioSrc.PlayOneShot(Ljud);
                break;


        }
    }
}
