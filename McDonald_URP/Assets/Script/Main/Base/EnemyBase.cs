using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    NavMeshAgent nav;
    Transform target;
    [SerializeField] GameObject DeathEffect;

    public List<SpawnManager.Weakness> Weak = new List<SpawnManager.Weakness>();
    public List<GameObject> WeakImage = new List<GameObject>();
    public float HP;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        target = SpawnManager.instance.player.transform;
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            var rand = Random.Range(0, 3);
            Weak.Add((SpawnManager.Weakness)rand);
        }
        var bar = Instantiate(SpawnManager.instance.EnemyBarPrefab,transform.position,Quaternion.identity);
        bar.GetComponent<EnemyBar>().Target = transform;
    }
    private void Update()
    {
        nav.SetDestination(target.position);
        HP -= Time.deltaTime;
        if(HP <= 0) isFailed();
    }
    void isFailed()
    {
        SpawnManager.instance.player.Damage();
        Destroy(gameObject);
    }
    public void Damage(int damage, SpawnManager.Weakness Weaked)
    {
        if (!isWeak(Weaked)) HP -= damage;
        else SearchWeak(Weaked);
        if (Weak.Count <= 0)
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            UIManager.instance.Score += 1;
            UIManager.instance.ScoreText.text = $"만족시킨 손놈 수 : {UIManager.instance.Score}";
            Destroy(gameObject);
        }
    }
    bool isWeak(SpawnManager.Weakness value)
    {
        foreach (var w in Weak)
        {
            if (w == value)
            {
                return true;
            }
        }
        return false;
    }
    void SearchWeak(SpawnManager.Weakness value)
    {
        for (int i = 0; i < Weak.Count; i++)
        {
            if (Weak[i] == value)
            {
                Weak.RemoveAt(i);
                Destroy(WeakImage[i]);
                WeakImage.RemoveAt(i);
                return;
            }
        }

    }
}
