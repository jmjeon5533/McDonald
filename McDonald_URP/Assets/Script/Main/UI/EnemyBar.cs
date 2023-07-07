using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBar : MonoBehaviour
{
    [HideInInspector] public Transform Target;
    [SerializeField] Transform foodPos;
    [SerializeField] SpriteRenderer barGauge;
    [HideInInspector] public EnemyBase enemyBase;
    private float enemyMaxHP;


    private void Start()
    {
        enemyBase = Target.GetComponent<EnemyBase>();
        enemyMaxHP = enemyBase.HP;
        for (int i = 0; i < enemyBase.Weak.Count; i++)
        {
            var weak = Instantiate(SpawnManager.instance.WeakPrefab[(int)enemyBase.Weak[i].Weakness], foodPos);
            enemyBase.WeakImage.Add(weak);
            var xPos = 0f;
            if(enemyBase.Weak.Count != 1)
            {
                xPos = Mathf.Lerp(1f,-1f,(float)i / (float)(enemyBase.Weak.Count - 1));
            }
            else
            {
                xPos = enemyBase.Weak.Count;
            }
            weak.transform.localPosition = new Vector3(xPos,0,0);
        }
    }
    private void Update()
    {
        if (Target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            barGauge.size = new Vector2((enemyBase.HP / enemyMaxHP) * 4.6f, barGauge.size.y);

            transform.position = Target.position + new Vector3(0, 2, 0);
        }
    }
}
