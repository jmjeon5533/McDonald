using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public float radius = 3f;
    public Vector3 dir;
    public float MoveSpeed;
    [HideInInspector] public int Damage;
    [SerializeField] GameObject DeathEffect;
    protected virtual void Start()
    {
        Destroy(gameObject, 5f);
    }
    protected virtual void Update()
    {
        transform.position += (dir * MoveSpeed * Time.deltaTime);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        // 충돌한 콜라이더를 처리하는 로직 작성
        foreach (Collider collider in colliders)
        {
            // 충돌한 콜라이더에 대한 처리 코드 작성
            if(collider.CompareTag("Player"))
            {
                collider.GetComponent<FirstPersonController>().Damage(Damage);
                Explode();
            }
            else if(collider.CompareTag("Map"))
            {
                Explode();
            }
        }
    }
    protected virtual void Explode()
    {
        Instantiate(DeathEffect,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position,radius);    
    }
}
