using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpriteBar : MonoBehaviour
{
    public SpriteRenderer HPBar;
    private Boss1 bossBase;

    [HideInInspector] public Transform Target;
    [SerializeField] Transform foodPos;
    [SerializeField] SpriteRenderer barGauge;
    [HideInInspector]

    void Start()
    {
        bossBase = Target.GetComponent<Boss1>();
        bossBase.MaxHP = bossBase.HP;
        if (!SceneManager.instance.FireMod)
        {
            bossBase.enemyMaxLimit = bossBase.limit;
            InitWeakUI();
        }
    }
    public void InitWeakUI()
    {
        for (int i = 0; i < bossBase.Weak.Count; i++)
        {
            var weak = Instantiate(SpawnManager.instance.WeakPrefab[(int)bossBase.Weak[i].Weakness], foodPos);
            bossBase.WeakImage.Add(weak);
            var xPos = 0f;
            if (bossBase.Weak.Count != 1)
            {
                xPos = Mathf.Lerp(3f, -3f, (float)i / (float)(bossBase.Weak.Count - 1));
            }
            else
            {
                xPos = bossBase.Weak.Count;
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
            if(SceneManager.instance.FireMod)
            {
                barGauge.size = new Vector2(0, barGauge.size.y);
            }
            else barGauge.size 
                = new Vector2((bossBase.limit / bossBase.enemyMaxLimit) * 4.6f, barGauge.size.y);

            HPBar.size = new Vector2((bossBase.HP / bossBase.MaxHP) * 4.6f, HPBar.size.y);

            transform.position = Target.position + new Vector3(0, 2, 0);
        }

    }
}
