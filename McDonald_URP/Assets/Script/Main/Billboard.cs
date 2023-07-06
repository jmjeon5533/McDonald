using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    private void Start()
    {
        // 카메라의 Transform 컴포넌트를 가져옴
        cameraTransform = Camera.main.transform;

    }

    private void LateUpdate()
    {
        // 객체가 항상 카메라를 향하도록 회전 설정
        transform.LookAt(cameraTransform.position);
    }
}
