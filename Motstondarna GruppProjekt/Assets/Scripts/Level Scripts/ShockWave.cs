using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField] float horizontalSpeed;
    [SerializeField] float verticlaSpeed;

    [SerializeField] float verticalTarget;

    void FixedUpdate()
    {
        Vector3 newPos = transform.position;

        newPos.z += horizontalSpeed * Time.fixedDeltaTime;

        newPos.y = Mathf.Lerp(newPos.y,verticalTarget,0.125f);

        transform.position = newPos;
    }
}
