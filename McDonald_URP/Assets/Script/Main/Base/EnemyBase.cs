using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ParticleSystemJobs;

public class EnemyBase : MonoBehaviour
{
    protected NavMeshAgent nav;
    protected Transform target;
    public GameObject DeathUIEffect;
    public GameObject DeathEffect;
    public int Score;

    public List<Weak> Weak = new List<Weak>();
    public List<GameObject> WeakImage = new List<GameObject>();
    public float limit;

    protected int WeakCount;
    protected bool isRand = true;
    protected GameObject Bar; //enemyUIBar
    public float enemyMaxLimit; //최대 한계(UI용)

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }
    protected virtual void Start()
    {
        target = SpawnManager.instance.Ronald.transform;
        InitWeak();
        limit = limit * (1 / SpawnManager.instance.hardValue[SceneManager.instance.StageNum]);
        nav.SetDestination(target.position);
    }
    protected void InitWeak()
    {
        var repeat = isRand ? Random.Range(1, 4) : WeakCount;
        for (int i = 0; i < repeat; i++)
        {
            var Weakrand = Random.Range(0, SpawnManager.instance.EnemyWeakCount[SceneManager.instance.StageNum]); //약점 부여
            var Valueweak = 1; //Random.Range(0, 3); //약점 개수
            Weak weak = new Weak();
            weak.Weakness = (SpawnManager.Weakness)Weakrand;
            weak.Value = (SpawnManager.Weakness)Weakrand //weakrand가 콜라일 경우 value * 60
                == SpawnManager.Weakness.Cola ? Valueweak * 30 : Valueweak;
            weak.MaxValue = weak.Value;
            Weak.Add(weak);
        }
    }
    private void Update()
    {
        if (SceneManager.instance.isGame) limit -= Time.deltaTime;
        else
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (limit <= 0 || Vector3.Distance(transform.position,target.position) <= 1.5f) isFailed();
    }
    protected virtual void isFailed()
    {
        SpawnManager.instance.player.Damage();
        Destroy(gameObject);
    }
    public void Damage(float damage, SpawnManager.Weakness Weaked)
    {
        if (!isWeak(Weaked)) limit -= damage;
        else SearchWeak(Weaked);
        if (Weak.Count <= 0) WeakOut();
    }
    protected virtual void WeakOut()
    {
        UIManager.instance.Score += Score;
        foreach (var score in UIManager.instance.ScoreText) score.text = string.Format("{0:N0}", UIManager.instance.Score);
    }
    protected virtual bool isWeak(SpawnManager.Weakness value)
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
    protected virtual void SearchWeak(SpawnManager.Weakness value)
    {
        for (int i = 0; i < Weak.Count; i++)
        {
            if (Weak[i].Weakness == value)
            {
                Weak[i].Value--;
                InitWeakUI();
                if (Weak[i].Value <= 0)
                {
                    Weak.RemoveAt(i);
                    Instantiate(DeathUIEffect, WeakImage[i].transform.position, Quaternion.identity);
                    Destroy(WeakImage[i]);
                    WeakImage.RemoveAt(i);
                }
                return;
            }
        }
    }
    protected void InitWeakUI()
    {
        for (int i = 0; i < Weak.Count; i++)
        {
            if (Weak[i].Value <= 0 || Weak[i].Weakness != SpawnManager.Weakness.Cola) continue;
            WeakImage[i].GetComponent<SpriteRenderer>().size
            = new Vector2(10.24f, Mathf.Lerp(0, 10.24f, (Weak[i].MaxValue - Weak[i].Value) / Weak[i].MaxValue));
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        // Apply damage to self
        float damage = 0.1f; // 대미지 양 (원하는 값으로 수정)
        SpawnManager.Weakness weakness = SpawnManager.Weakness.Cola; // 적중한 파티클의 약점 (원하는 값으로 수정)
        Damage(damage, weakness);
    }


}
