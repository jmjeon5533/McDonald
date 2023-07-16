using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class French_fries : WeaponBase
{
    float WeaponDisx;
    float CamFieldLerp;
    protected override void Start()
    {
        base.Start();
        CamFieldLerp = 1;
        WeaponDisx = WeaponPos.localPosition.x;
    }
    protected override void Attack()
    {
        Vector3 mousePosition = Input.mousePosition;

        // 카메라의 시점을 기준으로 마우스 커서의 위치를 월드 좌표계로 변환
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // 총알의 방향 벡터 계산
            Vector3 direction = (hit.point - weaponArm.position).normalized;

            GameObject bullet = Instantiate(Bullet, weaponArm.position,
                Quaternion.Euler(new Vector3(
                    SpawnManager.instance.player.cam.eulerAngles.x,player.eulerAngles.y,0)));

            BulletBase bulletMovement = bullet.GetComponent<BulletBase>();
            bulletMovement.Damage = Damage;
            bulletMovement.MoveSpeed = BulletSpeed + ((1 - CamFieldLerp) * 50);
            bulletMovement.dir = direction;
        }
        Instantiate(BulletParticle, Muzzle.transform.position,
        player.rotation);
        base.Attack();
    }
    protected override void Update()
    {
        base.Update();
        WeaponPos.localPosition
            = new Vector3(WeaponPos.localPosition.x,
                -0.254f, Mathf.Lerp(WeaponPos.localPosition.z, -0.061f, 0.2f));

        CamFieldValue = Input.GetMouseButton(1) ? 45 : 60;
    }
    protected override void FieldSet()
    {
        base.FieldSet();
        CamFieldLerp = Mathf.InverseLerp(45, 60, cam.fieldOfView);
        WeaponPos.localPosition
        = new Vector3(CamFieldLerp
        * WeaponDisx, -0.254f, -0.061f);
    }
    public override IEnumerator CamTick()
    {
        float[] HorRand = { 1, -1 };
        WeaponPos.Rotate(new Vector3(0.2f, 0, HorRand[Random.Range(0, 2)]) * 250f * Time.deltaTime);
        WeaponPos.Translate(new Vector3(0, 0, -2f) * Time.deltaTime * 20);
        var controller = player.GetComponent<FirstPersonController>();
        controller.CamTickPos = 50f * Time.deltaTime;
        while (controller.CamTickPos >= -0.5f)
        {
            controller.CamTickPos -= Time.deltaTime * recovery;
            yield return null;
        }
        controller.CamTickPos = 0 * Time.deltaTime;
    }
}
