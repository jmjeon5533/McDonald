using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HambergerBullet : BulletBase
{
    [SerializeField] float RotateSpeed;
    [SerializeField] AudioClip ExplodeSound;

    protected override void Update()
    {
        base.Update();
        transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
        dir = new Vector3(dir.x, Mathf.Lerp(dir.y, -1, 0.005f), dir.z);
    }
    protected override void Explode()
    {
        if (SceneManager.instance.FireMod)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius * 20);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<EnemyBase>().Damage(Damage, bulletAttribute);
                }
            }
            SoundManager.instance.SetAudio(ExplodeSound, SoundManager.SoundState.SFX, false);
        }
        base.Explode();
    }
}
