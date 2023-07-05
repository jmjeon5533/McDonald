using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    NavMeshAgent nav;
    Transform target;
    [SerializeField] GameObject DeathEffect;
    [SerializeField] GameObject DeathUIEffect;
    public int Score;

    public List<Weak> Weak = new List<Weak>();
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
            var Weakrand = Random.Range(0, 3); //약점 부여
            var Valueweak = Random.Range(0, 3); //약점 개수
            Weak weak = new Weak();
            weak.Weakness = (SpawnManager.Weakness)Weakrand;
            weak.Value = (SpawnManager.Weakness)Weakrand //weakrand가 콜라일 경우 value * 60
                == SpawnManager.Weakness.Cola ? Valueweak * 30 : Valueweak;
            weak.MaxValue = weak.Value;
            Weak.Add(weak);
        }
        var bar = Instantiate(SpawnManager.instance.EnemyBarPrefab, transform.position, Quaternion.identity);
        bar.GetComponent<EnemyBar>().Target = transform;
        HP = HP * (1 / SpawnManager.instance.hardValue[SceneManager.instance.StageNum]);
    }
    private void Update()
    {
        nav.SetDestination(target.position);
        HP -= Time.deltaTime;
        if (HP <= 0) isFailed();
    }
    void isFailed()
    {
        SpawnManager.instance.player.Damage();
        Destroy(gameObject);
    }
    public void Damage(float damage, SpawnManager.Weakness Weaked)
    {
        if (!isWeak(Weaked)) HP -= damage;
        else SearchWeak(Weaked);
        if (Weak.Count <= 0)
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            UIManager.instance.Score += Score;
            UIManager.instance.ScoreText.text = string.Format("{0:N0}", UIManager.instance.Score);
            Destroy(gameObject);
        }
    }
    bool isWeak(SpawnManager.Weakness value)
    {
        for (int i = 0; i < Weak.Count; i++)
        {
            if (Weak[i].Weakness == value)
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
            if (Weak[i].Weakness == value)
            {
                Weak[i].Value--;
                if (Weak[i].Value <= 0)
                {
                    Weak.RemoveAt(i);
                    Instantiate(DeathUIEffect,WeakImage[i].transform.position,Quaternion.identity);
                    Destroy(WeakImage[i]);
                    WeakImage.RemoveAt(i);
                }
                return;
            }
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        Damage(0.5f, SpawnManager.Weakness.Cola);
        Destroy(other);
    }
}
