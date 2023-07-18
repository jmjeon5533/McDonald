using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1 : EnemyBase
{
    public bool isSkill;
    public float SkillSpeed;
    [SerializeField] GameObject RangeObj;
    [SerializeField] GameObject SkillParticle;
    [SerializeField] Transform particlePivot;
    [Header("보스바")]
    [SerializeField] GameObject BossBarPrefab;

    private Transform foodPos;
    private Image HPBar, LimitBar;
    private GameObject BossBar;
    [SerializeField] float SkillRange;
    protected override void Start()
    {
        BossBar = Instantiate(BossBarPrefab, transform.position, Quaternion.identity, SpawnManager.instance.canvas);
        HPBar = BossBar.transform.GetChild(0).GetComponent<Image>();
        LimitBar = BossBar.transform.GetChild(1).GetComponent<Image>();
        foodPos = BossBar.transform.GetChild(2).transform;

        BossBar.transform.localPosition = new Vector3(0, -150, 0);
        BossBar.transform.localScale = new Vector3(1,1,1);
        BossBar.transform.SetAsFirstSibling();
        if (!SceneManager.instance.FireMod)
        {
            isRand = false;
            WeakCount = SpawnManager.instance.BossWeakCount[SceneManager.instance.StageNum];
        }
        else
        {
            nav.speed = nav.speed * 3;
            HP = HP * 200;
        }
        MaxHP = HP;
        if (!SceneManager.instance.FireMod)
        {
            InitWeak();
            limit = limit * (1 / SpawnManager.instance.hardValue[SceneManager.instance.StageNum]);
            nav.SetDestination(target.position);
        }
        if (!SceneManager.instance.FireMod)
        {
            enemyMaxLimit = limit;
            InitWeakPos();
        }
        RangeObj.SetActive(false);
    }
    protected override void Update()
    {
        bool isfire = SceneManager.instance.FireMod;
        base.Update();
        if (SceneManager.instance.FireMod)
        {
            LimitBar.fillAmount = 0;
        }
        else
        {
            float N = isfire ? HP : limit;
            float MaxN = isfire ? MaxHP : enemyMaxLimit;
            
            LimitBar.fillAmount = N / MaxN;

        }
        if(BossBar != null)
        {
            HPBar.fillAmount = HP / MaxHP;
        }
    }
    protected override void WeakOut()
    {
        base.WeakOut();
        HP--;
        if (HP <= 0)
        {
            Destroy(BossBar);
            SpawnManager.instance.GameClear();
        }
        else
        {
            limit = enemyMaxLimit;
            InitWeak();
            InitWeakPos();
        };
    }
    public void InitWeakPos()
    {
        for (int i = 0; i < Weak.Count; i++)
        {
            var weak = Instantiate(SpawnManager.instance.WeakUIPrefab[(int)Weak[i].Weakness], foodPos);
            WeakImage.Add(weak);
            var xPos = 0f;
            if (Weak.Count != 1)
            {
                xPos = Mathf.Lerp(-150f, 150f, (float)i / (float)(Weak.Count - 1));
            }
            else
            {
                xPos = Weak.Count;
            }
            weak.GetComponent<RectTransform>().sizeDelta = new Vector2(50,50);
            weak.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,0.5f);
            weak.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
            var index = 0;
            if(Weak[i].Weakness == SpawnManager.Weakness.Cola)
            {
                index = 1;
                var rect = weak.transform.GetChild(0).GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(50,50);
                rect.pivot = new Vector2(0.5f,0.5f);
                rect.localPosition = Vector2.zero;
            }
            var colaWeakChild = weak.transform.GetChild(index);
            colaWeakChild.gameObject.SetActive(false);
            weak.transform.localPosition = new Vector3(xPos, 0, 0);
        }
    }
    protected override void Failed()
    {
        Destroy(BossBar);
        SpawnManager.instance.GameOver();
    }
    protected override void InitWeakUI()
    {
        for (int i = 0; i < Weak.Count; i++)
        {
            if (Weak[i].Value <= 0 || Weak[i].Weakness != SpawnManager.Weakness.Cola) continue;
            WeakImage[i].transform.GetChild(0).GetComponent<Image>().fillAmount 
            = (Weak[i].MaxValue - Weak[i].Value) / Weak[i].MaxValue;
        }
    }
    protected override void Movement()
    {
        if (!isSkill)
        {
            if (Vector3.Distance(transform.position, target.position) <= 20)
            {
                isSkill = true;
                nav.ResetPath();
                StartCoroutine(Skill());
            }
            else
            {
                base.Movement();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 2, 1), SkillRange);
    }
    IEnumerator Skill()
    {
        Vector3 TargetDir = (target.position - transform.position);
        RangeObj.SetActive(true);
        transform.eulerAngles
            = new Vector3(0, Mathf.Atan2(TargetDir.x, TargetDir.z) * Mathf.Rad2Deg, 0);
        var mypos = transform.position;
        var pos = new Vector3(target.position.x, transform.position.y, target.position.z);
        yield return new WaitForSeconds(0.7f);
        RangeObj.SetActive(false);
        nav.enabled = false;
        print($"{mypos} : {pos}");
        while (Vector3.Distance(transform.position, pos) >= 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, SkillSpeed * Time.deltaTime);
            Collider[] colliders = Physics.OverlapSphere(transform.position + new Vector3(0, 2, 1), SkillRange);

            print("a");
            // 충돌한 콜라이더를 처리하는 로직 작성
            foreach (Collider collider in colliders)
            {
                print("b");
                // 충돌한 콜라이더에 대한 처리 코드 작성
                if (collider.CompareTag("Player"))
                {
                    collider.GetComponent<FirstPersonController>().Damage(damage);
                }
            }
            print("c");
            yield return null;
        }
        Instantiate(SkillParticle, particlePivot.position, Quaternion.identity);
        nav.enabled = true;
        yield return new WaitForSeconds(1.5f);
        isSkill = false;
    }
    protected override bool isWeak(SpawnManager.Weakness value)
    {
        return Weak[0].Weakness == value;
    }
    protected override void SearchWeak(SpawnManager.Weakness value)
    {
        if (Weak[0].Weakness == value)
        {
            Weak[0].Value--;
            InitWeakUI();
            if (Weak[0].Value <= 0)
            {
                Weak.RemoveAt(0);
                Instantiate(DeathUIEffect, WeakImage[0].transform.position, Quaternion.identity);
                Destroy(WeakImage[0]);
                WeakImage.RemoveAt(0);
            }
            return;
        }
    }
}
