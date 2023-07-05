using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
public class Weak
{
    public SpawnManager.Weakness Weakness;
    public int Value;
    public int MaxValue;
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }
    public FirstPersonController player;

    public GameObject EnemyPrefab;
    public GameObject EnemyBarPrefab;
    public List<Transform> EnemySpawnPos = new List<Transform>();

    public GameObject[] MapPrefab;
    public float[] hardValue;
    public float[] GameTime;
    [SerializeField] float min,sec;
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
        int StageNum = SceneManager.instance.StageNum;
        SpawnTime = SpawnTime * (1 / SpawnManager.instance.hardValue[SceneManager.instance.StageNum]);
        var map = Instantiate(MapPrefab[StageNum], new Vector3(0, 0, 0), Quaternion.identity);
        map.GetComponent<NavMeshSurface>().RemoveData();
        map.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    private void Update()
    {
        Spawn();
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
}