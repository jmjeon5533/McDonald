using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : EnemyBase
{
    public bool isAttack;
    public float AttackSpeed;
    [SerializeField] GameObject RangeObj;
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
            HP *= 50;
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
        if (!isAttack)
        {
            if (Vector3.Distance(transform.position, target.position) <= 20)
            {
                isAttack = true;
                nav.ResetPath();
                StartCoroutine(Attack());
            }
            else
            {
                base.Movement();
            }
        }
    }
    IEnumerator Attack()
    {
        Vector3 TargetDir = (target.position - transform.position);
        RangeObj.SetActive(true);
        transform.eulerAngles
            = new Vector3(0, Mathf.Atan2(TargetDir.x, TargetDir.z) * Mathf.Rad2Deg, 0);
        var mypos = transform.position;
        var pos = new Vector3(target.position.x,transform.position.y,target.position.z);
        yield return new WaitForSeconds(1);
        RangeObj.SetActive(false);
        nav.enabled = false;
        print($"{mypos} : {pos}");
        while(Vector3.Distance(transform.position,pos) >= 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position,pos,AttackSpeed * Time.deltaTime);
            yield return null;
        }
        nav.enabled = true;
        yield return new WaitForSeconds(2);
        isAttack = false;
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
