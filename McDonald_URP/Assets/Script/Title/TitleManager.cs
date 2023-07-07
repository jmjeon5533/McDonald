using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public static TitleManager instance { get; private set; }
    [SerializeField] Transform[] CamPos;
    [SerializeField] Transform CamArm;
    [SerializeField] float CamRotSpeed;
    [Space(10)]
    [SerializeField] GameObject[] TitleUI;
    [SerializeField] Button[] StageButton;
    void Start()
    {
        CamArm.position = CamPos[Random.Range(0, CamPos.Length)].position;
        StartCoroutine(SceneManager.instance.FadeIn());
        InitTab(0);

        for(int i = 0; i < StageButton.Length; i++)
        {
            var index = i;
            StageButton[i].onClick.AddListener(()=>SceneManager.instance.StageLoad(index));
        }
    }
    void Update()
    {
        CamArm.Rotate(new Vector3(0, CamRotSpeed, 0) * Time.deltaTime);
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
