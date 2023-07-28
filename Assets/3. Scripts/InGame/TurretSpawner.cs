using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;

    public int getSpawnSize() { return spawnPoints.Length; }

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    public void Spawn(int i_TurretIndex, int i_spawnIndex)
    {
        GameObject turret = GameManager.gm.turretPool.Get(i_TurretIndex);
        turret.transform.position = spawnPoints[i_spawnIndex].transform.position;
    }
}
