using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss2 : EnemyBase
{
    public bool isSkill;
    public float SkillSpeed;
    public float SwordEnergyCooltime;
    public bool isEnergy = false;
    [SerializeField] GameObject RangeObj;
    [SerializeField] GameObject SwordEnergy;
    [SerializeField] Transform particlePivot;
    [Header("보스바")]
    [SerializeField] GameObject BossBarPrefab;
    [Header("피격판정")]
    [SerializeField] Transform foot; //오른발
    [SerializeField] Transform Sword; //검

    [SerializeField] Transform AttackPos; //현재 공격 위치

    [SerializeField] AnimationClip[] animations;

    private Transform foodPos;
    private Image HPBar, LimitBar;
    private GameObject BossBar;
    [SerializeField] float SkillRange;
    [SerializeField] AudioClip SwordSound,FireSound;
    protected override void Start()
    {
        BossBar = Instantiate(BossBarPrefab, transform.position, Quaternion.identity, SpawnManager.instance.canvas);
        HPBar = BossBar.transform.GetChild(0).GetComponent<Image>();
        LimitBar = BossBar.transform.GetChild(1).GetComponent<Image>();
        foodPos = BossBar.transform.GetChild(2).transform;

        BossBar.transform.localPosition = new Vector3(0, -150, 0);
        BossBar.transform.localScale = new Vector3(1, 1, 1);
        BossBar.transform.SetAsFirstSibling();
        anim = GetComponent<Animator>();
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
        anim.speed = nav.speed / 3.5f;
        MaxHP = HP;
        if (!SceneManager.instance.FireMod)
        {
            InitWeak();
            limit = limit * (1 / SpawnManager.instance.hardValue[SceneManager.instance.StageNum]);
            anim.SetInteger("state", 1);
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
        if (BossBar != null)
        {
            HPBar.fillAmount = HP / MaxHP;
        }
        if(isSkill)
        {
            Collider[] colliders = Physics.OverlapSphere(AttackPos.position, SkillRange);
            // 충돌한 콜라이더를 처리하는 로직 작성
            foreach (Collider collider in colliders)
            {
                // 충돌한 콜라이더에 대한 처리 코드 작성
                if (collider.CompareTag("Player"))
                {
                    collider.GetComponent<FirstPersonController>().Damage(damage);
                }
            }
        }
    }    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPos.position, SkillRange);
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
            weak.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            weak.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            weak.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            var index = 0;
            if (Weak[i].Weakness == SpawnManager.Weakness.Cola)
            {
                index = 1;
                var rect = weak.transform.GetChild(0).GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(50, 50);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.localPosition = Vector2.zero;
            }
            var WeakChild = weak.transform.GetChild(index);
            WeakChild.gameObject.SetActive(false);
            weak.transform.localPosition = new Vector3(xPos, 0, 0);
        }
    }
    protected override void Failed()
    {
        Destroy(BossBar);
        SpawnManager.instance.GameOver();
    }
    protected override void Movement()
    {
        if (!isSkill)
        {
            Vector3 TargetDir = (target.position - transform.position);
            var dis = Vector3.Distance(transform.position, target.position);
            if (dis <= 30)
            {
                if (isEnergy)
                {
                    if (dis <= 14)
                    {
                        if (dis <= 7)
                        {
                            transform.eulerAngles
                            = new Vector3(0, Mathf.Atan2(TargetDir.x, TargetDir.z) * Mathf.Rad2Deg, 0);
                            AttackPos = foot;
                            SkillRange = 1;
                            StartCoroutine(Kick());
                        }
                        else
                        {
                            transform.eulerAngles
                            = new Vector3(0, Mathf.Atan2(TargetDir.x, TargetDir.z) * Mathf.Rad2Deg, 0);
                            AttackPos = Sword;
                            SkillRange = 3;
                            isSkill = true;
                            nav.ResetPath();
                            RandSkill();
                        }
                    }
                    else
                    {
                        anim.SetInteger("state", 1);
                        base.Movement();
                    }
                }
                else
                {
                    transform.eulerAngles
                    = new Vector3(0, Mathf.Atan2(TargetDir.x, TargetDir.z) * Mathf.Rad2Deg, 0);
                    AttackPos = Sword;
                    SkillRange = 3;
                    StartCoroutine(Upper_Slash());
                }
            }
            else
            {
                anim.SetInteger("state", 1);
                base.Movement();
            }
            transform.eulerAngles
            = new Vector3(0, Mathf.Atan2(TargetDir.x, TargetDir.z) * Mathf.Rad2Deg, 0);
        }
    }

    void RandSkill()
    {
        var rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0: StartCoroutine(Under_Slash()); break;
            case 1: StartCoroutine(Upper_Slash()); break;
            case 2: StartCoroutine(Foward_Slash()); break;
        }
    }
    IEnumerator Kick()
    {
        isSkill = true;
        anim.SetTrigger("kick");
        yield return new WaitForSeconds(animations[0].length);
        isSkill = false;
    }
    IEnumerator Under_Slash()
    {
        isSkill = true;
        anim.SetTrigger("under_Slash");
        yield return new WaitForSeconds(1);
        SoundManager.instance.SetAudio(SwordSound,SoundManager.SoundState.SFX,false);
        yield return new WaitForSeconds(animations[1].length - 1);
        isSkill = false;
    }
    IEnumerator Upper_Slash()
    {
        isSkill = true;
        anim.SetTrigger("upper_Slash");
        StartCoroutine(Count());
        yield return new WaitForSeconds(1);
        Vector3 TargetDir = (target.position - Sword.position);
        Vector3 SpawnPos = new Vector3(particlePivot.position.x,6.5f,particlePivot.position.z);
        var bullet = Instantiate(SwordEnergy,SpawnPos, Quaternion.Euler(
            new Vector3(0, Mathf.Atan2(TargetDir.x, TargetDir.z) * Mathf.Rad2Deg, 0) + new Vector3(0,90,90)));
        SoundManager.instance.SetAudio(FireSound,SoundManager.SoundState.SFX,false);
        bullet.GetComponent<EnemyFire>().dir = TargetDir.normalized;
        bullet.GetComponent<EnemyFire>().Damage = 50;
        yield return new WaitForSeconds(animations[2].length - 1);
        isSkill = false;
    }
    IEnumerator Foward_Slash()
    {
        isSkill = true;
        anim.SetTrigger("foward_Slash");
        yield return new WaitForSeconds(1);
        SoundManager.instance.SetAudio(SwordSound,SoundManager.SoundState.SFX,false);
        yield return new WaitForSeconds(animations[3].length - 1);
        isSkill = false;
    }
    IEnumerator Count()
    {
        print("energy");
        isEnergy = true;
        yield return new WaitForSeconds(SwordEnergyCooltime);
        isEnergy = false;
    }
    protected override bool isWeak(SpawnManager.Weakness value)
    {
        return Weak[0].Weakness == value;
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
