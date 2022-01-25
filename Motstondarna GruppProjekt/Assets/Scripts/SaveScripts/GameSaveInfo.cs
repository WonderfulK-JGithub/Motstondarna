using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveInfo : MonoBehaviour,ISaveable//K-J
{
    public int coinCount;
    public int levelProgress;

    public int currentLevel;
    public int currentCheckPoint;

    //spara data
    public object CaptureState()
    {
        return new SaveData
        {
            coinCount = coinCount,
            levelProgress = levelProgress,
            currentLevel = currentLevel,
            currentCheckPoint = currentCheckPoint,
        };

    }
    //ladda data
    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        coinCount = saveData.coinCount;
        coinCount = saveData.coinCount;
        coinCount = saveData.coinCount;
        coinCount = saveData.coinCount;
    }

    [System.Serializable]
    struct SaveData //Spardata
    {
        public int coinCount;
        public int levelProgress;

        public int currentLevel;
        public int currentCheckPoint;
    }
}
