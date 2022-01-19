using System.Collections.Generic;
using UnityEngine;

public class GroundSettings : MonoBehaviour
{
    public bool wallCollision = false; // man kan ändra detta i inspektorn - Anton
    List<Vector3> groundCoords = new List<Vector3>(); // hämtar koordinaterna för varje markobjekt
    Transform[] faces; // alla sidor
    void Start()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z)); // avrundar positionen (onödig kod för alla positioner är redan avrundade) - Anton
        GroundSettings[] grounds = FindObjectsOfType<GroundSettings>(); // hittar alla markobjekt i scenen - Anton
        foreach (var ground in grounds)
        {
            groundCoords.Add(ground.transform.position); // lägger till koordinaterna i groundCoords - Anton
        }
        faces = GetComponentsInChildren<Transform>(); // hämtar alla sidors Transform - Anton

        for (int i = 1; i < faces.Length; i++)
        {
            faces[i].GetComponent<MeshCollider>().enabled = wallCollision;
        }

        if (groundCoords.Contains(transform.position + new Vector3(2, 0, 0))) { Destroy(faces[1].gameObject); } // dessa rader sköter bortplockning av onödiga sidor, dvs sidor som tittar in i en annan sida. - Anton
        if (groundCoords.Contains(transform.position + new Vector3(-2, 0, 0))) { Destroy(faces[2].gameObject); }
        if (groundCoords.Contains(transform.position + new Vector3(0, 0, 2))) { Destroy(faces[3].gameObject); }
        if (groundCoords.Contains(transform.position + new Vector3(0, 0, -2))) { Destroy(faces[4].gameObject); }
    }
}
