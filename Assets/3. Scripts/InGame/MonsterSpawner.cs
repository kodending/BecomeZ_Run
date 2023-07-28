using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;

    public int getSpawnSize() { return spawnPoints.Length; }

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    public void Spawn(int i_monsterIndex, int i_spawnIndex)
    {
        GameObject enemy = GameManager.gm.monsterPool.Get(i_monsterIndex);
        enemy.transform.position = spawnPoints[i_spawnIndex].transform.position;
    }
}
