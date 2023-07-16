using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpriteBar : MonoBehaviour
{
    [HideInInspector] public Transform Target;
    [SerializeField] Transform foodPos;
    [SerializeField] SpriteRenderer barGauge;
    [HideInInspector] public EnemyBase enemyBase;

    void Start()
    {
        enemyBase = Target.GetComponent<EnemyBase>();
        if (SceneManager.instance.FireMod)
        {
            enemyBase.MaxHP = enemyBase.HP;
        }
        else
        {
            enemyBase.enemyMaxLimit = enemyBase.limit;
            InitWeakUI();
        }
    }
    public void InitWeakUI()
    {
        for (int i = 0; i < enemyBase.Weak.Count; i++)
        {
            var weak = Instantiate(SpawnManager.instance.WeakPrefab[(int)enemyBase.Weak[i].Weakness], foodPos);
            enemyBase.WeakImage.Add(weak);
            var xPos = 0f;
            if (enemyBase.Weak.Count != 1)
            {
                xPos = Mathf.Lerp(1f, -1f, (float)i / (float)(enemyBase.Weak.Count - 1));
            }
            else
            {
                xPos = enemyBase.Weak.Count;
            }
            weak.transform.localPosition = new Vector3(xPos, 0, 0);
        }
    }
    void Update()
    {
        if (Target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            bool isfire = SceneManager.instance.FireMod;
            float N = isfire ? enemyBase.HP : enemyBase.limit;
            float MaxN = isfire ? enemyBase.MaxHP : enemyBase.enemyMaxLimit;
            
            barGauge.size = new Vector2((N / MaxN) * 4.6f, barGauge.size.y);
            transform.position = Target.position + new Vector3(0, 3.5f, 0);
        }
    }
}
