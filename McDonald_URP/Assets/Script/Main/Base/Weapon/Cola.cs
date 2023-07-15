using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cola : WeaponBase
{
    public GameObject ColaWeaponPrefab;
    public ParticleSystem ColaWeapon;
    protected override void Attack()
    {
        if (Input.GetMouseButton(0))
        {
            ColaWeapon.Play();
            ColaWeapon.loop = true;
        }
        else
        {
            ColaWeapon.loop = false;
        }
    }
    protected override void Start()
    {
        base.Start();
        transform.localPosition = new Vector3(0.6f,-0.75f);
        transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
        ColaWeapon = Instantiate(ColaWeaponPrefab,transform.position,Quaternion.identity,transform)
        .GetComponent<ParticleSystem>();
        ColaWeapon.Stop();
        ColaWeapon.transform.localRotation = Quaternion.Euler(new Vector3(-2,-2.5f));
    }
    public override IEnumerator CamTick()
    {
        var controller = player.GetComponent<FirstPersonController>();
        controller.CamTickPos = 75f * Time.deltaTime;
        while (controller.CamTickPos >= -0.75f)
        {
            controller.CamTickPos -= Time.deltaTime * recovery;
            yield return null;
        }
        controller.CamTickPos = 0 * Time.deltaTime;
    }
}
