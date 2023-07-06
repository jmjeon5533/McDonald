using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    public TextMeshPro[] ScoreText;
    public Transform WeaponPanel;
    public Transform EndPanel;
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
        foreach(var score in ScoreText) score.text = $"{0}";
    }
    public void MinusHeartUI(int hp)
    {
        HeartUI[hp].SetActive(false);
    }
    public void InitWeaponUI(int Index)
    {
        for (int i = 0; i < WeaponUI.Count; i++)
        {
            WeaponUI[i].DOScale(1f, 0.2f).SetEase(Ease.OutSine);
        }
        WeaponUI[Index].DOScale(1.5f, 0.2f).SetEase(Ease.OutSine);
    }
    public void UseEndPanel(bool use)
    {
        EndPanel.DOLocalMoveY(use ? 0 : 380,1).SetEase(Ease.OutBack);
    }
}
