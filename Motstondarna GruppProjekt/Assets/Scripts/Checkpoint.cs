using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointID;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerPrefs.GetInt("progress", 0) < checkpointID)
            {
                PlayerPrefs.SetInt("progress", checkpointID);
                print("proggers");
            }
        }
    }
}
