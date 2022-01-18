using UnityEngine;

public class PlayerCheckpointSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("progress", 0) != 0)
        {
            Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.checkpointID == PlayerPrefs.GetInt("progress"))
                {
                    transform.position = checkpoint.transform.position;
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
