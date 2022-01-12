using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    Transform platform; // plattformen - Anton
    [Range(0, 5)] // hastigheten ska inte vara f�r h�g - Anton
    public float speed; // hastigheten - Anton
    public bool backAndForth; // om plattformen ska r�ra sig fram och tillbaka - Anton
    bool reverse; // anv�nds endast om backAndForth �r satt p� true och plattformen �ker tillbaka - Anton
    Transform[] children; // f�r tag p� alla children f�r att den senare ska extrahera ut punkterna - Anton
    Transform[] points; // alla punkter (de ber�ttar f�r plattformen hur den ska �ka) - Anton
    int targetPoint = 0; // vilken punkt plattan r�r sig mot - Anton
    public List<Transform> objectsOnPlatform = new List<Transform>(); // vilka objekt som nuddar plattformen - Anton

    private void Start()
    {
        children = GetComponentsInChildren<Transform>(); // f�r tag p� alla children - Anton
        platform = children[1]; // plattformen �r f�rsta childen - Anton
        points = new Transform[children.Length - 2]; // antalet punkter �r points l�ngd utan parenten och plattformen - Anton
        for (int i = 2; i < children.Length; i++)
        {
            points[i - 2] = children[i]; // l�gger in punkterna i points-arrayen
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
