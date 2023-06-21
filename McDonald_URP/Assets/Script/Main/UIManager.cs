using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    public Text ScoreText;
    public Transform WeaponPanel;
    public Image DamagedPanel;
    public List<Transform> WeaponUI = new List<Transform>();
    public List<GameObject> HeartUI = new List<GameObject>();
    public int Score = 0;
    [SerializeField] FirstPersonController player;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        for(int i = 0; i < HeartUI.Count; i++)
        {
            HeartUI[i].SetActive(true);
        }
    }
    public void MinusHeartUI(int hp)
    {
        if(hp <= 0)
        {
            print("죽었슴");
            return;
        }
        StartCoroutine(DamagedEffect());
        HeartUI[hp].SetActive(false);
    }
    IEnumerator DamagedEffect()
    {
        float a = 0;
        while(DamagedPanel.color.a < 0.4f)
        {
            a += Time.deltaTime * 1.5f;
            DamagedPanel.color = new Color(1,0,0,a); 
            yield return null;
        }
        while(DamagedPanel.color.a > 0f)
        {
            a -= Time.deltaTime * 1.5f;
            DamagedPanel.color = new Color(1,0,0,a); 
            yield return null;
        }
    }
    public void InitWeaponUI(int Index)
    {
        for (int i = 0; i < WeaponUI.Count; i++)
        {
            WeaponUI[i].DOScale(1f, 0.2f).SetEase(Ease.OutSine);
        }
        WeaponUI[Index].DOScale(1.5f, 0.2f).SetEase(Ease.OutSine);
    }
}
