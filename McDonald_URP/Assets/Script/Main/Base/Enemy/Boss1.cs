using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : EnemyBase
{
    public int HP;
    protected override void Start()
    {
        isRand = false;
        WeakCount = SpawnManager.instance.BossWeakCount[SceneManager.instance.StageNum];
        Bar = Instantiate(SpawnManager.instance.BossBarPrefab, transform.position, Quaternion.identity);
        Bar.GetComponent<BossSpriteBar>().Target = transform;
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
    protected override void isFailed()
    {
        SpawnManager.instance.GameOver();
    }
}
