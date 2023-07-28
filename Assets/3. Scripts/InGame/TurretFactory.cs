using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFactory : MonoBehaviour
{
    //float m_fTimer;
    //int maxIdx = 2;
    //int curIdx = 0;

    //// Update is called once per frame
    //void Update()
    //{
    //    m_fTimer += Time.deltaTime;

    //    if (curIdx < maxIdx)
    //    {
    //        if (m_fTimer > 4.7f)
    //        {
    //            m_fTimer = 0;
    //            int turretSize = GameManager.gm.turretPool.GetTurretSize();
    //            int spwanSize = GameManager.gm.turretSpawner.getSpawnSize();

    //            //첫번째는 자신의 오브젝트 위치도 포함되기 때문에 자식 오브젝트 위치만 꺼내기위해 1부터 시작
    //            GameManager.gm.turretSpawner.Spawn(Random.Range(0, turretSize), Random.Range(1, spwanSize));

    //            curIdx++;
    //        }
    //    }
    //}
    int spawnSize;

    void Start()
    {
        spawnSize = GameManager.gm.turretSpawner.getSpawnSize();
    }

    public void SpawnTurrets(int i_iSpawnCnt, int i_turretIdx)
    {
        for (int idx = 0; idx < i_iSpawnCnt; idx++)
        {
            GameManager.gm.turretSpawner.Spawn(i_turretIdx - 1, Random.Range(1, spawnSize));
        }
    }
}
