using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
public class Weak
{
    public SpawnManager.Weakness Weakness;
    public float Value;
    public float MaxValue;
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }
    public FirstPersonController player;

    public GameObject EnemyPrefab; //적 프리팹
    public GameObject EnemyBarPrefab; //적 UI 프리팹
    public List<Transform> EnemySpawnPos = new List<Transform>(); //적 소환 위치

    public GameObject[] MapPrefab;
    public float[] hardValue;
    public float[] GameTime;
    [SerializeField] int min;
    [SerializeField] float sec;
    public Transform canvas;

    [SerializeField] float SpawnTime;
    float SpawnCurtime;

    public enum Weakness
    {
        Hamberger,
        Cola,
        French_fries
    }
    public List<GameObject> WeakPrefab = new List<GameObject>();

    private void Start()
    {
        Debug.Log(SceneManager.instance.StageNum);
        int StageNum = SceneManager.instance.StageNum;
        SpawnTime = SpawnTime * (1 / SpawnManager.instance.hardValue[SceneManager.instance.StageNum]);
        var map = Instantiate(MapPrefab[StageNum], new Vector3(0, 0, 0), Quaternion.identity);
        map.GetComponent<NavMeshSurface>().RemoveData();
        map.GetComponent<NavMeshSurface>().BuildNavMesh();

        min = (int)(GameTime[SceneManager.instance.StageNum] / 60);
        sec = GameTime[SceneManager.instance.StageNum] % 60;

        SceneManager.instance.isGame = true;
    }
    private void Update()
    {
        if (SceneManager.instance.isGame)
        {
            Spawn();
            Timer();
        }
    }
    void Spawn()
    {
        SpawnCurtime += Time.deltaTime;
        if (SpawnCurtime >= SpawnTime)
        {
            SpawnCurtime -= SpawnTime;
            var rand = Random.Range(0, EnemySpawnPos.Count);
            Instantiate(EnemyPrefab, EnemySpawnPos[rand].position, Quaternion.identity);
        }
    }
    void Timer()
    {
        sec -= Time.deltaTime;
        if (sec <= 0)
        {
            if (min <= 0)
            {
                GameClear();
                return;
            }
            sec = 60;
            min--;
        }
        UIManager.instance.TimerText.text = $"{min}:{(int)sec}";
    }
    public void GameClear()
    {
        SceneManager.instance.isGame = false;
        UIManager.UsePanel(UIManager.instance.EndPanel,true,DG.Tweening.Ease.OutBack,1);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        for(int i = 0; i < player.WeaponObj.Count; i++)
        {
            player.WeaponObj[i].SetActive(false);
        }
    }
    public void GameOver()
    {
        SceneManager.instance.isGame = false;
        for(int i = 0; i < player.WeaponObj.Count; i++)
        {
            player.WeaponObj[i].SetActive(false);
        }
    }
}