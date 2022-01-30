using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] Transform[] levelCameraPoints;
    [SerializeField] TextMeshProUGUI[] levelNumbers;
    [SerializeField] Transform ball;
    [SerializeField] Vector3 ballOffsett;

    [SerializeField] Color finishedColor;
    [SerializeField] Color unlockedColor;
    [SerializeField] Color lockedColor;

    [SerializeField] float rotateSpeed;
    [SerializeField] float transitionWait;
    [SerializeField] float unlockTime;
    [SerializeField] ParticleSystem unlockPS;

    HubCamera cam;

    int levelIndex;

    int unlockedLevel = -1;

    public int levelProgress;

    bool hasSelected;

    float unlockTimer;

    LevelSelectState state;

    void Awake()
    {
        cam = FindObjectOfType<HubCamera>();

        
    }

    void Start()
    {
        state = LevelSelectState.Selecting;

        GameSaveInfo.currentLevel = 0;
        levelProgress = GameSaveInfo.current.levelProgress;

        for (int i = 0; i < levelNumbers.Length; i++)
        {
            if (i < levelProgress)
            {
                levelNumbers[i].color = finishedColor;
            }
            else if (i == levelProgress)
            {
                levelNumbers[i].color = unlockedColor;
            }
            else
            {
                levelNumbers[i].color = lockedColor;
            }
        }

        if (GameSaveInfo.currentLevel != -1 && levelProgress == GameSaveInfo.currentLevel)
        {
            GameSaveInfo.current.levelProgress++;
            levelProgress++;

            UnlockLevel(levelProgress);
        }
    }

    void Update()
    {
        switch(state)
        {
            case LevelSelectState.Selecting:
                #region
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    levelIndex++;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    levelIndex--;
                }

                levelIndex = Mathf.Clamp(levelIndex, 0, levelCameraPoints.Length - 1);

                cam.targetPoint = levelCameraPoints[levelIndex];

                levelNumbers[levelIndex].transform.localRotation *= Quaternion.Euler(0f, rotateSpeed * Time.deltaTime, 0f);

                ball.transform.position = levelCameraPoints[levelIndex].position - ballOffsett;

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (levelIndex <= levelProgress)
                    {
                        LevelSelected();
                    }
                }
                #endregion
                break;
            case LevelSelectState.Unlocking:

                unlockTimer -= Time.deltaTime;

                if(unlockTimer <= 0f)
                {
                    levelNumbers[unlockedLevel].color = unlockedColor;
                    unlockPS.transform.position = levelNumbers[unlockedLevel].transform.position;
                    unlockPS.Play();
                    state = LevelSelectState.Selecting;
                    levelIndex = unlockedLevel;
                }
                else
                {
                    levelNumbers[unlockedLevel].transform.localRotation *= Quaternion.Euler(0f, rotateSpeed * Time.deltaTime * 10f, 0f);
                }

                break;
        }
    }

    void LevelSelected()
    {
        foreach (var item in levelNumbers)
        {
            item.enabled = false;
        }

        BallMovement ball = FindObjectOfType<BallMovement>();
        ball.state = PlayerState.Hub;
        ball.rb.velocity = new Vector3(11f, 0f, 0f);

        state = LevelSelectState.Off;

        Invoke("EnterLevel", transitionWait);
    }

    void EnterLevel()
    {
        SceneTransition.current.EnterScene(levelIndex + 0);
    }
    
    void UnlockLevel(int level)
    {
        levelNumbers[level-1].color = finishedColor;

        state = LevelSelectState.Unlocking;
        unlockedLevel = level;

        print(level);

        cam.targetPoint = levelCameraPoints[level];
        unlockTimer = unlockTime;
    }
}


public enum LevelSelectState
{
    Unlocking,
    Selecting,
    Off,
}