using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    public TextMeshProUGUI[] ScoreText;
    public TextMeshProUGUI TimerText;
    public Transform WeaponPanel;
    public Transform EndPanel;
    public Transform PausePanel;
    public Button Title, Next;
    public List<Transform> WeaponUI = new List<Transform>();
    public List<GameObject> HeartUI = new List<GameObject>();
    public int Score = 0;

    public bool Escape = false;
    [SerializeField] FirstPersonController player;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        var Scene = SceneManager.instance;
        Button[] pauseButton = new Button[3];
        for (int i = 1; i < PausePanel.childCount; i++)
        {
            print(i);
            pauseButton[i-1] = PausePanel.GetChild(i).GetComponent<Button>();
        }

        for (int i = 0; i < HeartUI.Count; i++)
        {
            HeartUI[i].SetActive(true);
        }
        foreach (var score in ScoreText) score.text = $"{0}";
        Title.onClick.AddListener(() => { Scene.SceneLoad(0); });
        Next.onClick.AddListener(() =>
        {
            Scene.StageNum++;
            Scene.SceneLoad(1);
        });

        //pauseButton[0].onClick.AddListener();
        pauseButton[2].onClick.AddListener(() =>
        {
            Debug.Log(2);
            Escape = false;
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            UsePanel(PausePanel, Escape, Ease.OutSine, 0.5f);
        });

        pauseButton[1].onClick.AddListener(() =>
        {
            Debug.Log(1);
            SceneManager.instance.SceneLoad(0);
        });
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Escape = !Escape;
            if (Escape)
            {
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            UsePanel(PausePanel, Escape, Ease.OutSine, 0.5f);

        }
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
    public static void UsePanel(Transform Obj, bool use, Ease easing, float time)
    {
        Obj.DOLocalMoveY(use ? 0 : 380, time).SetEase(easing);
    }
}
