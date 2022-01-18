using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject[] windows;
    public void OpenWindow(int window)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;

        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }
        windows[window].SetActive(true);
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;

        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void FullRestart()
    {
        PlayerPrefs.SetInt("progress", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (windows[0].activeInHierarchy == true)
            {
                Resume();
            }
            else
            {
                OpenWindow(0);
            }
        }
    }
}