using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    private Transform cameraTr;
    private Vector3 targetPos;

    void Start()
    {
        cameraTr = Camera.main.transform;
    }

    void Update()
    {
        targetPos = new Vector3(cameraTr.position.x, transform.position.y, cameraTr.position.z);
        this.transform.LookAt(targetPos);
        transform.Rotate(0f, 180f, 0f);
    }
}
