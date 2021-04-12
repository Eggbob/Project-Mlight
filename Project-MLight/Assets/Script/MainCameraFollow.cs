using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraFollow : MonoBehaviour
{
    public Transform target; // 따라다닐 대상 트랜스폼 컴포넌트
    public Vector3 offset; // 카메라 위치

    private void Update()
    {
        transform.position = target.position + offset;
    }
}
