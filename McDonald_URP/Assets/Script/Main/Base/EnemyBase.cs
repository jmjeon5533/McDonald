using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ParticleSystemJobs;

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
            var Weakrand = Random.Range(0, SpawnManager.instance.WeakCount[SceneManager.instance.StageNum]); //약점 부여
            var Valueweak = 1;//Random.Range(0, 3); //약점 개수
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
        if (SceneManager.instance.isGame) HP -= Time.deltaTime;
        else
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

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
            foreach(var score in UIManager.instance.ScoreText) score.text = string.Format("{0:N0}", UIManager.instance.Score);
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
    void InitWeakUI()
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
