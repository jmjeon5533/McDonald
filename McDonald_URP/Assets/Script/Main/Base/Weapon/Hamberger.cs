using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamberger : WeaponBase
{
    [SerializeField] Transform Muzzle;
    [SerializeField] GameObject BulletParticle;
    protected override void Attack()
    {
        if (Input.GetMouseButtonDown(0))
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
                bulletMovement.dir = direction;
            }
            Instantiate(BulletParticle,Muzzle.transform.position,
            player.rotation);
            base.Attack();
        }
    }
}
