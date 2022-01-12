using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public Transform platform;
    [Range(0, 5)]
    public float speed;
    public bool backAndForth;
    bool reverse;
    Transform[] children;
    Transform[] points;
    int targetPoint = 0;
    public List<Transform> objectsOnPlatform = new List<Transform>();

    private void Start()
    {
        children = GetComponentsInChildren<Transform>();
        points = new Transform[children.Length - 2];
        for (int i = 2; i < children.Length; i++)
        {
            points[i - 2] = children[i];
        }
    }
    private void FixedUpdate()
    {
        Vector3 difference = platform.transform.position - Vector3.MoveTowards(platform.transform.position, points[targetPoint].position, speed * Time.fixedDeltaTime);
        platform.transform.position -= difference;

        for (int i = 0; i < objectsOnPlatform.Count; i++)
        {
            objectsOnPlatform[i].position -= difference;
        }

        if (platform.transform.position == points[targetPoint].position)
        {
            if (backAndForth)
            {

                if (targetPoint >= points.Length - 1) { reverse = true; }
                else if (targetPoint <= 0) { reverse = false; }

                if (!reverse)
                {
                    targetPoint++;
                }
                else
                { 
                    targetPoint--;
                }
            }
            else
            {
                targetPoint++;
                if (targetPoint >= points.Length) { targetPoint = 0; }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            objectsOnPlatform.Add(collision.transform);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            objectsOnPlatform.Remove(collision.transform);
        }
    }
}
