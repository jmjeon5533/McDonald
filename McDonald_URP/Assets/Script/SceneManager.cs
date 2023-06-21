using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    [SerializeField] Image FadeImage;
    [SerializeField] float FadeSpeed;
    public static SceneManager instance { get; private set; }
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
    }
    public void StageLoad(int index)
    {
        StageNum = index;
        StartCoroutine(Fade2Load("Main"));
    }
    public IEnumerator FadeIn()
    {
        float a = 1;
        while(a > 0)
        {
            a -= Time.deltaTime * FadeSpeed;
            yield return null;
            FadeImage.color = new Color(0,0,0,a);
            if(a <= 0) break;
        }
    }
    public IEnumerator Fade2Load(string SceneName)
    {
        float a = 0;
        while(a < 1)
        {
            a += Time.deltaTime * FadeSpeed;
            yield return null;
            FadeImage.color = new Color(0,0,0,a);
            if(a >= 1) break;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
