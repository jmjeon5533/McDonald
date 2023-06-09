using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float MoveSpeed;
    public int Damage;
    [SerializeField] GameObject DeathEffect;
    void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        transform.Translate(transform.forward * MoveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBase>().Damage(Damage);
            Instantiate(DeathEffect,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
