using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] Transform[] levelCameraPoints;
    [SerializeField] TextMeshProUGUI[] levelNumbers;
    [SerializeField] Transform ramp;
    [SerializeField] float rampDistance;

    [SerializeField] Color finnishedColor;
    [SerializeField] Color unlockedColor;
    [SerializeField] Color lockedColor;

    [SerializeField] float rotateSpeed;

    HubCamera cam;

    int levelIndex;

    public int levelProgress;

    void Awake()
    {
        cam = FindObjectOfType<HubCamera>();

        for (int i = 0; i < levelNumbers.Length; i++)
        {
            if(i < levelProgress)
            {
                levelNumbers[i].color = finnishedColor;
            }
            else if(i == levelProgress)
            {
                levelNumbers[i].color = unlockedColor;
            }
            else
            {
                levelNumbers[i].color = lockedColor;
            }
        }
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            levelIndex++;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            levelIndex--;
        }

        levelIndex = Mathf.Clamp(levelIndex, 0, levelCameraPoints.Length - 1);

        cam.targetPoint = levelCameraPoints[levelIndex];

        levelNumbers[levelIndex].transform.localRotation *= Quaternion.Euler(0f, rotateSpeed * Time.deltaTime,0f);

        ramp.transform.position = levelCameraPoints[levelIndex].position - new Vector3(rampDistance,0.5f,0f);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            foreach (var item in levelNumbers)
            {
                item.enabled = false;
            }

            BallMovement ball = FindObjectOfType<BallMovement>();
            ball.state = PlayerState.Hub;
            ball.rb.velocity = new Vector3(11f, 0f, 0f);
        }
    }

    
}
