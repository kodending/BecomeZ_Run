using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    //float m_fTimer;
    //int maxIdx = 50;
    //int curIdx = 0;

    //void Update()
    //{
    //m_fTimer += Time.deltaTime;

    //if(curIdx <= maxIdx)
    //{
    //    if (m_fTimer > 3)
    //    {
    //        m_fTimer = 0;
    //        int monsterSize = GameManager.gm.monsterPool.getMonsterSize();
    //        int spwanSize = GameManager.gm.monsterSpawner.getSpawnSize();

    //        GameManager.gm.monsterSpawner.Spawn(Random.Range(0, monsterSize), Random.Range(1, spwanSize));

    //        curIdx++;
    //    }
    //}
    //}

    int spawnSize;
    void Start()
    {
        spawnSize = GameManager.gm.monsterSpawner.getSpawnSize();
    }

    public void SpawnMonsters(int i_iSpawnCnt, int i_monsterIdx)
    { 
        for(int idx = 0; idx < i_iSpawnCnt; idx++)
        {
            GameManager.gm.monsterSpawner.Spawn(i_monsterIdx - 1, Random.Range(1, spawnSize));
        }
    }
}
