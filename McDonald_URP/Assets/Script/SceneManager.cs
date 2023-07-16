using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class SceneManager : MonoBehaviour
{
    [SerializeField] Image FadeImage;
    [SerializeField] float FadeSpeed;
    public Transform OptionPanel;
    public static SceneManager instance { get; private set; }
    public bool isGame = false; //게임 중 유무
    public bool FireMod = false; //사격 모드 유무
    [SerializeField] Button FireModButton;
    [SerializeField] Image FireModImage;
    [SerializeField] Sprite[] FireModSprite = new Sprite[2];
    [SerializeField] TextMeshProUGUI FireModText, FireModExplain;

    [SerializeField] Slider BGMSl,SFXSl;
    public Transform canvas;
    public int StageNum = 0;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(canvas);
    }
    private void Start()
    {
        StageNum = 0;
        FireModButton.onClick.AddListener(() =>
        {
            FireMod = !FireMod;
            FireModImage.sprite = FireMod ? FireModSprite[0] : FireModSprite[1];
            InitText();
        });

        InitText();
    }
    private void Update()
    {
        FireModButton.gameObject.SetActive(!isGame);
        SoundManager.instance.BGMVolume = BGMSl.value;
        SoundManager.instance.SFXVolume = SFXSl.value;
    }
    public void InitText()
    {
        FireModText.text = FireMod ? "난사 모드" : "약점 모드";
        FireModExplain.text
            = FireMod ? "약점 대신 체력을 깎는 사격 중심 모드입니다." :
                "약점을 깎아 적을 처리하는 순발력 중심 모드입니다.";
    }

    public void SceneLoad(int index)
    {
        StartCoroutine(Fade2Load(index));
    }
    public void StageLoad(int index)
    {
        StageNum = index;
        StartCoroutine(Fade2Load(1));
        isGame = true;
    }
    public IEnumerator FadeIn()
    {
        CloseOption();
        float a = 1;
        Time.timeScale = 1;
        while (a > 0)
        {
            a -= Time.unscaledDeltaTime * FadeSpeed;
            yield return null;
            FadeImage.color = new Color(0, 0, 0, a);
            if (a <= 0) break;
        }
    }
    public IEnumerator FadeOut()
    {
        CloseOption();
        float a = 0;
        while (a < 1)
        {
            a += Time.unscaledDeltaTime * FadeSpeed;
            yield return null;
            FadeImage.color = new Color(0, 0, 0, a);
            if (a >= 1) break;
        }
    }
    public IEnumerator Fade2Load(int index)
    {
        CloseOption();
        float a = 0;
        while (a < 1)
        {
            a += Time.unscaledDeltaTime * FadeSpeed;
            yield return null;
            FadeImage.color = new Color(0, 0, 0, a);
            if (a >= 1) break;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        StartCoroutine(FadeIn());
    }
    public void OpenOption()
    {
        SceneManager.UsePanel(OptionPanel, true, Ease.OutSine, 0.5f);
    }
    public void CloseOption()
    {
        SceneManager.UsePanel(OptionPanel, false, Ease.OutSine, 0.5f);
    }
    public static void UsePanel(Transform Obj, bool use, Ease easing, float time)
    {
        Obj.DOLocalMoveY(use ? 0 : 380, time).SetEase(easing);
    }
}
