using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    protected override void Start()
    {
        WeakCount = SpawnManager.instance.EnemyWeakCount[SceneManager.instance.StageNum];
        Bar = Instantiate(SpawnManager.instance.EnemyBarPrefab, transform.position, Quaternion.identity);
        Bar.GetComponent<EnemySpriteBar>().Target = transform;
        base.Start();
    }
    protected override void WeakOut()
    {
        base.WeakOut();
        Instantiate(DeathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
