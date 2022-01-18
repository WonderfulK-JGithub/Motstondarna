using System.Collections.Generic;
using UnityEngine;

public class GroundSettings : MonoBehaviour
{
    public bool wallCollision = false;
    List<Vector3> groundCoords = new List<Vector3>();
    Transform[] faces;
    void Start()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        GroundSettings[] grounds = FindObjectsOfType<GroundSettings>();
        foreach (var ground in grounds)
        {
            groundCoords.Add(ground.transform.position);
        }
        faces = GetComponentsInChildren<Transform>();

        if (groundCoords.Contains(transform.position + new Vector3(2, 0, 0))) { Destroy(faces[1].gameObject); }
        if (groundCoords.Contains(transform.position + new Vector3(-2, 0, 0))) { Destroy(faces[2].gameObject); }
        if (groundCoords.Contains(transform.position + new Vector3(0, 0, 2))) { Destroy(faces[3].gameObject); }
        if (groundCoords.Contains(transform.position + new Vector3(0, 0, -2))) { Destroy(faces[4].gameObject); }
    }
}
