using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float RotateSpeed;
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed,Space.World);
    }
}
