using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    public static TitleManager instance { get; private set; }
    [SerializeField] Transform[] CamPos;
    [SerializeField] Transform CamArm;
    [SerializeField] Transform cam;
    [SerializeField] float CamRotSpeed;
    [Space(10)]
    [SerializeField] GameObject[] TitleUI;
    [SerializeField] Button[] StageButton;
    private bool isIntro = false;
    void Start()
    {
        cam = CamArm.GetChild(0);
        StartCoroutine(SceneManager.instance.FadeIn());
        InitTab(0);
        StageButton[0].onClick.AddListener(()=>
        {
            StartCoroutine(Intro());
        });
        for(int i = 1; i < StageButton.Length; i++)
        {
            var index = i;
            StageButton[i].onClick.AddListener(()=>SceneManager.instance.StageLoad(index));
        }
    }
    IEnumerator Intro()
    {
        yield return StartCoroutine(SceneManager.instance.FadeOut());
        isIntro = true;
        for(int i = 0; i < TitleUI.Length; i++)
        {
            TitleUI[i].SetActive(false);
        }
        cam.SetParent(null);
        cam.position = new Vector3(-17.4f,15.5f,33.88f);
        cam.eulerAngles = new Vector3(28, 154);
        StartCoroutine(SceneManager.instance.FadeIn());
        cam.DOMove(new Vector3(-13.43f, 10.55f, 25.64f),3);
        print("Complete!");
    }
    void Update()
    {
        if(!isIntro)
        {
            CamArm.Rotate(new Vector3(0, CamRotSpeed, 0) * Time.deltaTime);
        }
    }
    public void InitTab(int value)
    {
        for(int i = 0; i < TitleUI.Length; i++)
        {
            TitleUI[i].SetActive(false);
        }
        TitleUI[value].SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
