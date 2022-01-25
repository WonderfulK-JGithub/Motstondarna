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
                SceneTransition.current.ReLoadScene();
            }
        }
    }
}
