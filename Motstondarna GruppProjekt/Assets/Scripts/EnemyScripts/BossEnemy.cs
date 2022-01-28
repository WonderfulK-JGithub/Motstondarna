using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public static BossEnemy current;


    [SerializeField] Vector3 bossKnockback;

    private void Awake()
    {
        current = this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            BallHealth.current.BossDamaged(bossKnockback);
            BossManager.current.BossDamaged();
        }
    }
}
