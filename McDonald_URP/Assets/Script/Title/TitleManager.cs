using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.AI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    public static TitleManager instance { get; private set; }
    [SerializeField] Transform[] CamPos; //카메라 랜덤 위치
    [SerializeField] Transform CamArm; //카메라 피봇
    [SerializeField] Transform cam; //카메라
    [SerializeField] float CamRotSpeed; //카메라 회전 속도
    [Space(10)]
    [SerializeField] GameObject[] TitleUI; //타이틀에 들어갈 UI
    [SerializeField] Button[] TitleButton; //타이틀 버튼
    [SerializeField] Button[] StageButton; //스테이지 버튼들
    [SerializeField] Button[] TitleLoadButton; //타이틀로 이동하는 버튼
    [SerializeField] Transform OptionPanel; //설정 창
    [SerializeField] Button OptionButton; //설정 버튼
    [Space(10)]
    private bool isIntro = false; //인트로 유무
    [SerializeField] GameObject[] IntroEnemy; //인트로에 등장할 적
    [SerializeField] GameObject map; //인트로용 맵

    [SerializeField] Button FireModButton; //모드 전환 버튼
    [SerializeField] Image FireModImage; //모드 선택 이미지
    [SerializeField] Sprite[] FireModSprite = new Sprite[2]; //모드 선택 유무 스프라이트
    [SerializeField] TextMeshProUGUI FireModText, FireModExplain; //이름, 설명
    [SerializeField] Button IntroSkipButton; //인트로 스킵 버튼
    void Start()
    {
        var s = SoundManager.instance;
        var Scm = SceneManager.instance;
        map.GetComponent<NavMeshSurface>().RemoveData();
        map.GetComponent<NavMeshSurface>().BuildNavMesh();

        SceneManager.instance.isGame = false;

        FireModButton.onClick.AddListener(() =>
        {
            Scm.FireMod = !Scm.FireMod;
            FireModImage.sprite = Scm.FireMod ? FireModSprite[0] : FireModSprite[1];
            InitText();
            SoundManager.instance.SetAudio(Scm.ClickSound,
                SoundManager.SoundState.SFX, false);
        });
        IntroSkipButton.onClick.AddListener(() =>
        {
            SceneManager.instance.StageNum = 0;
            SceneManager.instance.SceneLoad(1);
            SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                SoundManager.SoundState.SFX, false);
        });
        IntroSkipButton.gameObject.SetActive(false);
        for (int i = 0; i < IntroEnemy.Length; i++)
        {
            IntroEnemy[i].SetActive(false);
        }
        cam = CamArm.GetChild(0);
        StartCoroutine(SceneManager.instance.FadeIn());
        InitTab(0);
        StageButton[0].onClick.AddListener(() =>
        {
            StartCoroutine(Intro());
            SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                SoundManager.SoundState.SFX, false);
        });
        for (int i = 1; i < StageButton.Length; i++)
        {
            var index = i;
            StageButton[index].onClick.AddListener(() =>
            {
                if (index < 3) SceneManager.instance.StageLoad(index);
                SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                    SoundManager.SoundState.SFX, false);
            });
        }
        OptionButton.onClick.AddListener(() =>
            {
                SceneManager.instance.OpenOption();
                SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                    SoundManager.SoundState.SFX, false);
            });
        s.SetAudio(s.BGM[Random.Range(0, s.BGM.Length)], SoundManager.SoundState.BGM, true);
        OptionPanel = SceneManager.instance.OptionPanel;

        for (int i = 0; i < TitleButton.Length; i++)
        {
            var index = i;
            TitleButton[index].onClick.AddListener(() =>
            {
                InitTab(index + 1);
                SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                    SoundManager.SoundState.SFX, false);
            });
        }
        for (int i = 0; i < TitleLoadButton.Length; i++)
        {
            var index = i;
            TitleLoadButton[index].onClick.AddListener(() =>
            {
                InitTab(0);
                SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                    SoundManager.SoundState.SFX, false);
            });
        }

        InitText();
    }
    IEnumerator Intro()
    {
        yield return StartCoroutine(SceneManager.instance.FadeOut());
        IntroSkipButton.gameObject.SetActive(true);
        for (int i = 0; i < IntroEnemy.Length; i++)
        {
            IntroEnemy[i].SetActive(true);
        }
        isIntro = true;
        for (int i = 1; i < TitleUI.Length - 1; i++)
        {
            TitleUI[i].SetActive(false);
        }
        cam.SetParent(null);
        cam.position = new Vector3(-17.4f, 15.5f, 33.88f);
        cam.eulerAngles = new Vector3(28, 154);
        StartCoroutine(SceneManager.instance.FadeIn());
        yield return cam.DOMove(new Vector3(-13.43f, 10.55f, 25.64f), 2).WaitForCompletion();
        cam.position = new Vector3(-22.54f, 2.54f, 31.46f);
        cam.eulerAngles = new Vector3(18, 158);
        yield return new WaitForSeconds(2f);
        cam.position = new Vector3(-20.88f, 3.47f, 27.77f);
        cam.eulerAngles = new Vector3(18, 297);
        yield return new WaitForSeconds(1f);
        IntroEnemy[0].transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        IntroEnemy[0].transform.GetChild(0).gameObject.SetActive(false);
        cam.position = new Vector3(-6.5f, 9.91f, 33.15f);
        cam.eulerAngles = new Vector3(30, 180);
        for (int i = 0; i < IntroEnemy.Length; i++)
        {
            var nav = IntroEnemy[i].GetComponent<NavMeshAgent>();
            IntroEnemy[i].GetComponent<Animator>().speed = nav.speed;
            nav.SetDestination(new Vector3(0, 0, -3));
        }
        yield return new WaitForSeconds(3);
        SceneManager.instance.StageNum = 0;
        SceneManager.instance.SceneLoad(1);
        print("Complete!");
        IntroSkipButton.gameObject.SetActive(false);
    }
    public void InitText()
    {
        FireModText.text = SceneManager.instance.FireMod ? "난사 모드" : "약점 모드";
        FireModExplain.text
            = SceneManager.instance.FireMod ? "약점 대신 체력을 깎는 사격 중심 모드입니다." :
                "약점을 깎아 적을 처리하는 순발력 중심 모드입니다.";
    }
    void Update()
    {
        FireModButton.gameObject.SetActive(!SceneManager.instance.isGame);
        if (!isIntro)
        {
            CamArm.Rotate(new Vector3(0, CamRotSpeed, 0) * Time.deltaTime);
        }
    }
    public void InitTab(int value)
    {
        for (int i = 0; i < TitleUI.Length; i++)
        {
            TitleUI[i].SetActive(false);
        }
        TitleUI[value].SetActive(true);
    }
    public void Exit()
    {
        SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
                SoundManager.SoundState.SFX, false);
        Application.Quit();
    }
}
