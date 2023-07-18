using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : EnemyBase
{
    public bool isSkill;
    public float SkillSpeed;
    [SerializeField] GameObject RangeObj;
    [SerializeField] GameObject SkillParticle;
    [SerializeField] Transform particlePivot;
    [SerializeField] float SkillRange;
    protected override void Start()
    {
        Bar = Instantiate(SpawnManager.instance.BossBarPrefab, transform.position, Quaternion.identity);
        Bar.GetComponent<BossSpriteBar>().Target = transform;
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

        if (!SceneManager.instance.FireMod)
        {
            InitWeak();
            limit = limit * (1 / SpawnManager.instance.hardValue[SceneManager.instance.StageNum]);
            nav.SetDestination(target.position);
        }
        RangeObj.SetActive(false);
    }
    protected override void WeakOut()
    {
        base.WeakOut();
        HP--;
        if (HP <= 0)
        {
            SpawnManager.instance.GameClear();
        }
        else
        {
            limit = enemyMaxLimit;
            InitWeak();
            Bar.GetComponent<BossSpriteBar>().InitWeakUI();
        };
    }
    protected override void Failed()
    {
        SpawnManager.instance.GameOver();
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
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0,2,1),SkillRange);    
    }
    IEnumerator Skill()
    {
        Vector3 TargetDir = (target.position - transform.position);
        RangeObj.SetActive(true);
        transform.eulerAngles
            = new Vector3(0, Mathf.Atan2(TargetDir.x, TargetDir.z) * Mathf.Rad2Deg, 0);
        var mypos = transform.position;
        var pos = new Vector3(target.position.x, transform.position.y, target.position.z);
        yield return new WaitForSeconds(1);
        RangeObj.SetActive(false);
        nav.enabled = false;
        print($"{mypos} : {pos}");
        while (Vector3.Distance(transform.position, pos) >= 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, SkillSpeed * Time.deltaTime);
            Collider[] colliders = Physics.OverlapSphere(transform.position + new Vector3(0,2,1), SkillRange);

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
        yield return new WaitForSeconds(2);
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
