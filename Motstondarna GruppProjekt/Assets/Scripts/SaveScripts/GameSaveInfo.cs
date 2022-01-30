using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveInfo : MonoBehaviour,ISaveable//K-J
{
    public static GameSaveInfo current;

    public static int currentLevel = -1;

    public int coinCount;
    public int levelProgress;

    public int[] coinLevelsCount;

    private void Awake()
    {
        current = this;
    }

    //spara data
    public object CaptureState()
    {

        return new SaveData
        {
            levelProgress = levelProgress,
            coinLevelsCount = coinLevelsCount,
        };

    }
    //ladda data
    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        levelProgress = saveData.levelProgress;
        coinLevelsCount = saveData.coinLevelsCount;
    }

    [System.Serializable]
    struct SaveData //Spardata
    {
        public int levelProgress;
        public int[] coinLevelsCount;
    }
}
