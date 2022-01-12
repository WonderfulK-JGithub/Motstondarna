using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    Transform platform; // plattformen - Anton
    [Range(0, 5)] // hastigheten ska inte vara för hög - Anton
    public float speed; // hastigheten - Anton
    public bool backAndForth; // om plattformen ska röra sig fram och tillbaka - Anton
    bool reverse; // används endast om backAndForth är satt på true och plattformen åker tillbaka - Anton
    Transform[] children; // får tag på alla children för att den senare ska extrahera ut punkterna - Anton
    Transform[] points; // alla punkter (de berättar för plattformen hur den ska åka) - Anton
    int targetPoint = 0; // vilken punkt plattan rör sig mot - Anton
    public List<Transform> objectsOnPlatform = new List<Transform>(); // vilka objekt som nuddar plattformen - Anton

    private void Start()
    {
        children = GetComponentsInChildren<Transform>(); // får tag på alla children - Anton
        platform = children[1]; // plattformen är första childen - Anton
        points = new Transform[children.Length - 2]; // antalet punkter är points längd utan parenten och plattformen - Anton
        for (int i = 2; i < children.Length; i++)
        {
            points[i - 2] = children[i]; // lägger in punkterna i points-arrayen
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
