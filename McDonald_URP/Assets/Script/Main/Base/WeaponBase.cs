using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public GameObject Bullet;
    public Transform player;
    protected Transform weaponArm;
    protected virtual void Start()
    {
        player = SpawnManager.instance.player.transform;
        weaponArm = player.GetChild(0).GetChild(0);
    }
    void Update()
    {
        Attack();
    }
    protected abstract void Attack();
}
