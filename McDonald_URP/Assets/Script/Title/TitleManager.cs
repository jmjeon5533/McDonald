using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.AI;

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

    [SerializeField] GameObject[] IntroEnemy;
    [SerializeField] GameObject map;
    void Start()
    {
        map.GetComponent<NavMeshSurface>().RemoveData();
        map.GetComponent<NavMeshSurface>().BuildNavMesh();
        for(int i = 0; i < IntroEnemy.Length; i++)
        {   
            IntroEnemy[i].SetActive(false);
        }
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
        for(int i = 0; i < IntroEnemy.Length; i++)
        {   
            IntroEnemy[i].SetActive(true);
        }
        isIntro = true;
        for(int i = 0; i < TitleUI.Length; i++)
        {
            TitleUI[i].SetActive(false);
        }
        cam.SetParent(null);
        cam.position = new Vector3(-17.4f,15.5f,33.88f);
        cam.eulerAngles = new Vector3(28, 154);
        StartCoroutine(SceneManager.instance.FadeIn());
        yield return cam.DOMove(new Vector3(-13.43f, 10.55f, 25.64f),2).WaitForCompletion();
        cam.position = new Vector3(-22.54f,2.54f,31.46f);
        cam.eulerAngles = new Vector3(18, 158);
        yield return new WaitForSeconds(2f);
        cam.position = new Vector3(-20.88f,2.47f,27.77f);
        cam.eulerAngles = new Vector3(18, 297);
        yield return new WaitForSeconds(1f);
        IntroEnemy[0].transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        IntroEnemy[0].transform.GetChild(0).gameObject.SetActive(false);
        cam.position = new Vector3(-6.5f,9.91f,33.15f);
        cam.eulerAngles = new Vector3(30, 180);
        for(int i = 0; i < IntroEnemy.Length; i++)
        {   
            IntroEnemy[i].GetComponent<NavMeshAgent>().SetDestination(new Vector3(0,0,-3));
        }
        yield return new WaitForSeconds(3);
        SceneManager.instance.StageNum = 0;
        SceneManager.instance.SceneLoad(1);
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
