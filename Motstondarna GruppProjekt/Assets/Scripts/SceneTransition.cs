using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition current;

    int sceneIndex;

    Animator anim;
    void Awake()
    {
        current = this;
    }

    public void EnterScene(int _sceneIndex)
    {
        sceneIndex = _sceneIndex;
        anim.Play("SceneTransition_Exit");
    }


    void LoadScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
