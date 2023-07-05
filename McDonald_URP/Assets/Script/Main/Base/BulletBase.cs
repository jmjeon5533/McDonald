using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float radius = 1f;
    public Vector3 dir;
    public float MoveSpeed;
    public int Damage;
    [SerializeField] GameObject DeathEffect;
    public SpawnManager.Weakness bulletAttribute;
    void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        transform.Translate(dir * MoveSpeed * Time.deltaTime);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        // 충돌한 콜라이더를 처리하는 로직 작성
        foreach (Collider collider in colliders)
        {
            // 충돌한 콜라이더에 대한 처리 코드 작성
            if(collider.CompareTag("Enemy"))
            {
                collider.GetComponent<EnemyBase>().Damage(Damage, bulletAttribute);
                Instantiate(DeathEffect,transform.position,Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
