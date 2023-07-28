using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //기본 표본

    [SerializeField]
    private Transform[] spawnPoints;

    public int getSpawnSize() { return spawnPoints.Length; }

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void Spawn(int i_monsterIndex, int i_spawnIndex)
    {
        GameObject enemy = GameManager.gm.monsterPool.Get(i_monsterIndex);
        enemy.transform.position = spawnPoints[i_spawnIndex].transform.position;
    }
}
