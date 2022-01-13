using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Max Script
public class WanderingEnemy : MonoBehaviour
{
    [SerializeField] Vector3 wanderingAreaCenter;

    [SerializeField] float wanderingAreaRadius;

    [SerializeField] float wanderingSpeed;
    [SerializeField] float chasingSpeed;

    Vector3 currentTarget;

    Transform player;

    bool isChasingPlayer = false;

    void Start()
    {
        wanderingAreaCenter = transform.position;
        player = FindObjectOfType<BallMovement>().transform;
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        Vector3 target = isChasingPlayer ? player.position : currentTarget;
        float speed = isChasingPlayer ? chasingSpeed : wanderingSpeed;

        //Rigidbody.
    }

    void NewPos()
    {
        Vector3 newTarget = wanderingAreaCenter + 
            new Vector3
                (
                Random.Range(-wanderingAreaRadius, wanderingAreaRadius), 
                Random.Range(-wanderingAreaRadius, wanderingAreaRadius),
                0
                );

        currentTarget = newTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,255,0,0.4f);

        if (Application.isPlaying)
        {
            Gizmos.DrawSphere(wanderingAreaCenter, wanderingAreaRadius);
        }
        else
        {
            Gizmos.DrawSphere(transform.position, wanderingAreaRadius);

        }
    }
}
