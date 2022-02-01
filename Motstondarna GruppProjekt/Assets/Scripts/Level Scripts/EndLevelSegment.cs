using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelSegment : MonoBehaviour
{
    [SerializeField] List<BaseEnemy> goldenPins;

    bool levelEnded;
    void Update()
    {
        if(!levelEnded)
        {
            for (int i = 0; i < goldenPins.Count; i++)
            {
                if (goldenPins[i].hasDied)
                {
                    goldenPins.RemoveAt(i);
                    i--;
                }
            }


            if (goldenPins.Count == 0)
            {
                levelEnded = true;

                int i = 0;
                foreach (var item in FindObjectsOfType<CollectableCoin>())
                {
                    if(item.isCollected)
                    {
                        if (!item.isStored) GameSaveInfo.current.coinCount++;
                        item.isStored = true;
                        
                    }
                    if(item.isStored) i++;

                    item.isCollected = false;
                }

                GameSaveInfo.current.coinLevelsCount[GameSaveInfo.currentLevel] = i;

                SaveSystem.current.Save();
                PlayerPrefs.SetInt("progress", 0);


                SceneTransition.current.ReLoadScene();
            }
        }
    }
}
