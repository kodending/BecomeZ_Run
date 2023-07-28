using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;

    List<GameObject>[] pools;

    public int GetBulletSize() { return prefabs.Length; }

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        int poolSize = pools.Length;

        for (int idx = 0; idx < poolSize; idx++)
        {
            pools[idx] = new List<GameObject>();
        }
    }

    public GameObject Get(int i_idx, GameObject turret, Vector3 i_vTargetPos, Vector3 i_vStartPos)
    {
        GameObject select = null;

        foreach(GameObject item in pools[i_idx])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[i_idx], this.transform);
            pools[i_idx].Add(select);
        }

        return select;
    }
}
