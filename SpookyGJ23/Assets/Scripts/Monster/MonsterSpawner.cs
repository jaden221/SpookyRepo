using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;

    [SerializeField] Transform monsterPrefab;

    void Awake()
    {
        int index = Random.Range(0, spawnPoints.Length);
        
        Transform monsterInst = Instantiate(monsterPrefab, spawnPoints[index].position, Quaternion.identity);
    }
}
