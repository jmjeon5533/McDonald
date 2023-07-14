using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HambergerBullet : BulletBase
{
    [SerializeField] float RotateSpeed; 

    protected override void Update()
    {
        base.Update();
        transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
        dir = new Vector3(dir.x,Mathf.Lerp(dir.y,-1,0.005f),dir.z);
    }
}
