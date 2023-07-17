using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ParticleSystemJobs;

public class EnemyBase : MonoBehaviour
{
    protected NavMeshAgent nav;
    protected Animator anim;
    protected Transform target;
    public GameObject DeathUIEffect;
    public GameObject DeathEffect;
    public int Score;
    [Header("약점 모드")]
    public List<Weak> Weak = new List<Weak>();
    public List<GameObject> WeakImage = new List<GameObject>();
    public float limit; //인내심
    public float enemyMaxLimit; //인내심 최대(UI용)
    protected int WeakCount;
    protected bool isRand = true; //약점 갯수 랜덤 유무 (보스 구별용)
    [Header("사격 모드")]
    public float HP; //체력
    public float MaxHP; //최대 체력

    public float damage; //공격력

    protected GameObject Bar; //enemyUIBar

    private void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }
    protected virtual void Start()
    {
        anim.speed = nav.speed;
        if (SceneManager.instance.FireMod)
        {
            target = SpawnManager.instance.player.transform;
        }
        else
        {
            target = SpawnManager.instance.Ronald.transform;
            InitWeak();
            limit = limit * (1 / SpawnManager.instance.hardValue[SceneManager.instance.StageNum]);
            nav.SetDestination(target.position);
        }
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
    protected virtual void Update()
    {
        if (SceneManager.instance.isGame)
        {
            if (SceneManager.instance.FireMod)
            {
                nav.SetDestination(target.position);
            }
            else limit -= Time.deltaTime;
        }
        else
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (limit <= 0 || Vector3.Distance(transform.position, target.position) <= 2f) isFailed();
    }
    protected virtual void isFailed()
    {
        if (SceneManager.instance.FireMod)
        {
            SpawnManager.instance.player.Damage(damage);
        }
        else
        {
            SpawnManager.instance.player.Damage();
            Destroy(gameObject);
        }
    }
    public void Damage(float damage, SpawnManager.Weakness Weaked)
    {
        if (SceneManager.instance.FireMod)
        {
            HP -= damage;
            if(HP <= 0) WeakOut();
        }
        else
        {
            if (!isWeak(Weaked)) limit -= damage;
            else SearchWeak(Weaked);
            if (Weak.Count <= 0) WeakOut();
        }
    }
    protected virtual void WeakOut()
    {
        UIManager.instance.Score += Score;
        foreach (var score in UIManager.instance.ScoreText) 
            score.text = string.Format("{0:N0}", UIManager.instance.Score);
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
        float damage = SceneManager.instance.FireMod ? 1f : 0.1f; // 대미지 양 (원하는 값으로 수정)
        // Apply damage to self
        SpawnManager.Weakness weakness = SpawnManager.Weakness.Cola; // 적중한 파티클의 약점 (원하는 값으로 수정)
        Damage(damage, weakness);
    }


}
