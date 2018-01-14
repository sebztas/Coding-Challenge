using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    private void Start()
    {
        transform.LookAt(new Vector3(0f, 0f, transform.position.z));
    }
}
