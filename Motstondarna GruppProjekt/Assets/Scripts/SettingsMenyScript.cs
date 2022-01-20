using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenyScript : MonoBehaviour
{
    public void Settings() // Button till starta spelet
    {
        //SceneManager.LoadScene("SettingsMeny");
        SceneTransition.current.EnterScene(1);
    }
}
