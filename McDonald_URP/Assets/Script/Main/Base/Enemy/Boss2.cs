using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : EnemyBase
{
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
            HP *= 50;
        }
        base.Start();
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
