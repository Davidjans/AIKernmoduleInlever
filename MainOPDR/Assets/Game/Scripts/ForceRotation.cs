using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRotation : MonoBehaviour
{
    void Update()
    {
        transform.eulerAngles = new Vector3(90, 0, 0);
    }
}
