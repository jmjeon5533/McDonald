using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    public Text ScoreText;
    [SerializeField] Image HPBar;
    public List<Transform> WeaponUI = new List<Transform>();
    public int Score = 0;
    [SerializeField] FirstPersonController player;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        HPBar.fillAmount = player.HP / player.MaxHp;
    }
    public void InitWeaponUI(int Index)
    {
        for (int i = 0; i < WeaponUI.Count; i++)
        {
            WeaponUI[i].DOLocalMoveX(-50, 0.2f).SetEase(Ease.OutSine);
        }
        WeaponUI[Index].DOLocalMoveX(-100, 0.2f).SetEase(Ease.OutSine);
    }
}
