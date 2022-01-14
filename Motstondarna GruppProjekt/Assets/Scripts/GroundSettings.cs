using System.Collections.Generic;
using UnityEngine;

public class GroundSettings : MonoBehaviour
{
    public List<Vector3> groundCoords = new List<Vector3>();
    public Transform[] faces;
    void Start()
    {
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
