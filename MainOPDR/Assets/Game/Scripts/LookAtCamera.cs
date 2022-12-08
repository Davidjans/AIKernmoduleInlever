using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        Vector3 rotation = transform.localEulerAngles;
        rotation.y += 180;
        rotation.x += 180;
        transform.localEulerAngles = rotation;
    }
}
