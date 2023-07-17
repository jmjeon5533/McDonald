using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamberger : WeaponBase
{
    protected override void Start()
    {
        base.Start();
        AttackTime = SceneManager.instance.FireMod ? AttackTime * 2 : AttackTime;
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

            GameObject bullet = Instantiate(Bullet, weaponArm.position, Quaternion.identity);

            BulletBase bulletMovement = bullet.GetComponent<BulletBase>();
            bulletMovement.Damage = Damage;
            bulletMovement.MoveSpeed = BulletSpeed;
            bulletMovement.dir = direction;
        }
        Instantiate(BulletParticle, Muzzle.transform.position,
        player.rotation);
        base.Attack();
    }
    public override IEnumerator CamTick()
    {
        WeaponPos.Rotate(Vector3.left * 700f * Time.deltaTime);
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
