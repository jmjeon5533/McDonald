using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMap : MonoBehaviour
{
    public List<Transform> SpawnPos = new List<Transform>();

    public Transform Ronald;

    void Start()
    {
        var sp = SpawnManager.instance;
        
        sp.Ronald = Ronald;
        sp.EnemySpawnPos.Clear();
        foreach(var map in SpawnPos)
        {
            sp.EnemySpawnPos.Add(map);
        }
    }
}
