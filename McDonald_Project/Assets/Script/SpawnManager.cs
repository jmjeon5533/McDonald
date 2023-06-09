using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance {get; private set;}
    private void Awake() {
        instance = this;
    }
    public FirstPersonController player;
    
    public GameObject EnemyPrefab;
    public GameObject EnemyBarPrefab;
    public Transform[] SpawnPos;
    public Transform canvas;
    [SerializeField] float SpawnTime;
    float Curtime;

    public enum Weakness{
        Hamberger,
        Cola,
        French_fries
    }
    public List<GameObject> WeakPrefab = new List<GameObject>();

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
            var rand = Random.Range(0, SpawnPos.Length);
            Instantiate(EnemyPrefab, SpawnPos[rand].position, Quaternion.identity);
        }
    }
}