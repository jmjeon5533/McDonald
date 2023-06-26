using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public Transform canvas;
    [SerializeField] float SpawnTime;
    float Curtime;

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
        Curtime += Time.deltaTime;
        if (Curtime >= SpawnTime)
        {
            Curtime -= SpawnTime;
            var rand = Random.Range(0, EnemySpawnPos.Count);
            Instantiate(EnemyPrefab, EnemySpawnPos[rand].position, Quaternion.identity);
        }
    }
}