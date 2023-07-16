using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [Header("텍스트")]
    public TextMeshProUGUI[] ScoreText; //메인 스코어 & 결과창 스코어
    public TextMeshProUGUI TimerText; //상단 타이머
    public TextMeshProUGUI AmmoText; //현재 탄창
    public TextMeshProUGUI MaxAmmoText; //최대 탄창
    [Space(10)]
    [Header("패널")]
    public Transform WeaponPanel; //무기 패널
    public Transform megazinePanel; //탄창 패널
    public Transform ClearPanel; //결과창
    public Transform OverPanel; //게임 오버 창
    [SerializeField] Transform OptionPanel; //설정 창
    public Button Title1, Title2, Next, Retry; //타이틀로 돌아가기, 다음 스테이지, 재도전

    public Transform PausePanel; //퍼즈 탭
    public List<Transform> WeaponUI = new List<Transform>(); //적용된 무기들

    [SerializeField] GameObject[] HPPanel = new GameObject[2]; //모드 전환에 따른 패널 전환
    public List<GameObject> HeartUI = new List<GameObject>(); //햄버거 목숨
    public Image HPImage; //체력 목숨
    public int Score = 0; //스코어

    public bool Escape = false;
    [SerializeField] FirstPersonController player;
    [SerializeField] Image SelectWeaponImage; //장착 이미지
    public Image ReloadImage; //장전 UI
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
            pauseButton[i - 1] = PausePanel.GetChild(i).GetComponent<Button>();
        }
        OptionPanel = SceneManager.instance.OptionPanel;

        for (int i = 0; i < HeartUI.Count; i++)
        {
            HeartUI[i].SetActive(true);
        }
        foreach (var score in ScoreText) score.text = $"{0}";
        Title1.onClick.AddListener(() =>
        {
            Scene.SceneLoad(0);
            SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                SoundManager.SoundState.SFX, false);
        });
        Title2.onClick.AddListener(() =>
        {
            Scene.SceneLoad(0);
            SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                SoundManager.SoundState.SFX, false);
        });
        Next.onClick.AddListener(() =>
        {
            Scene.StageNum++;
            Scene.SceneLoad(1);
            SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                SoundManager.SoundState.SFX, false);
        });
        Retry.onClick.AddListener(() =>
        {
            Scene.SceneLoad(1);
            SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                SoundManager.SoundState.SFX, false);
        });
        //pauseButton[0].onClick.AddListener();
        pauseButton[2].onClick.AddListener(() =>
        {
            Escape = false;
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            SceneManager.UsePanel(PausePanel, Escape, Ease.OutSine, 0.5f);

            SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                SoundManager.SoundState.SFX, false);
        });

        pauseButton[1].onClick.AddListener(() =>
        {
            SceneManager.instance.SceneLoad(0);
            SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                SoundManager.SoundState.SFX, false);
        });

        pauseButton[0].onClick.AddListener(() =>
        {
            SceneManager.instance.OpenOption();
            SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                SoundManager.SoundState.SFX, false);
        });

        ReloadImage.enabled = false;
        InitAmmoUI();

        var active = SceneManager.instance.FireMod ? 0 : 1;
        for (int i = 0; i < HPPanel.Length; i++)
        {
            HPPanel[i].SetActive(false);
        }
        HPPanel[active].SetActive(true);

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
            SceneManager.UsePanel(PausePanel, Escape, Ease.OutSine, 0.5f);
        }
    }
    public void InitAmmoUI()
    {
        if (player.WeaponObj.Count <= 0)
        {
            megazinePanel.gameObject.SetActive(false);
        }
        else
        {
            megazinePanel.gameObject.SetActive(true);
            var weaponAmmo = player.WeaponObj[player.SelectWeaponNum].GetComponent<WeaponBase>();
            AmmoText.text = weaponAmmo.Ammo.ToString();
            MaxAmmoText.text = weaponAmmo.megazine.ToString();
            SelectWeaponImage.sprite = player.WeaponImage[player.SelectWeaponNum];
        }
    }
    public void MinusHeartUI(float hp)
    {
        if (SceneManager.instance.FireMod)
        {
            HPImage.fillAmount = hp / player.MaxHP;
        }
        else HeartUI[(int)hp].SetActive(false);
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
