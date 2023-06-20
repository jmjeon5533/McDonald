using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public static TitleManager instance { get; private set; }
    [SerializeField] Transform[] CamPos;
    [SerializeField] Transform CamArm;
    [SerializeField] float CamRotSpeed;

    [SerializeField] GameObject[] TitleUI;
    void Start()
    {
        CamArm.position = CamPos[Random.Range(0, CamPos.Length)].position;
        StartCoroutine(SceneManager.instance.FadeIn());
        TitleUI[0].SetActive(true);
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
}
