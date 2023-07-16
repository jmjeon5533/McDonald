using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    public Material[] materials;
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    protected override void Start()
    {
        meshRenderer.material = materials[Random.Range(0,materials.Length)];
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
