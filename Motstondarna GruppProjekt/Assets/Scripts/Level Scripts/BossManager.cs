using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] EnemyWave[] enemyWaves;

    
    [ContextMenu("SpawnEnemy")]
    void SpawnWave()
    {
        int currentWave = 0;

        for (int i = 0; i < enemyWaves[currentWave].enemiesToSpawn.Length; i++)
        {
            Instantiate(enemyWaves[currentWave].enemiesToSpawn[i], enemyWaves[currentWave].enemyPositions[i],Quaternion.identity);
        }
    }

}

[System.Serializable]
public class EnemyWave
{
    public GameObject[] enemiesToSpawn;
    public Vector3[] enemyPositions;
}