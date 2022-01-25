using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCoin : MonoBehaviour, ISaveable//K-J
{
    [SerializeField] Color normalColor;
    [SerializeField] Color alreadyCollectedColor;

    [SerializeField] bool isCollected;

    MeshRenderer rend;
    MeshCollider col;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        col = GetComponent<MeshCollider>();
    }

    //spara data
    public object CaptureState()
    {
        return new SaveData
        {
            isCollected = isCollected
        };
        
    }
    //ladda data
    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        isCollected = saveData.isCollected;

        //ger myntet en färg baserat på om den redan har tagits eller inte;
        if(isCollected)
        {
            rend.material.color = alreadyCollectedColor;
        }
        else
        {
            rend.material.color = normalColor;
        }
    }

    [System.Serializable] struct SaveData //Spardata
    {
        public bool isCollected;
    }


    public void CollectCoin()//när man nuddar myntet
    {
        rend.enabled = false;
        col.enabled = false;

        isCollected = true;
    }
}
