using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamRotate : MonoBehaviour
{
    public float RotateSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime,Space.World);
    }
}
