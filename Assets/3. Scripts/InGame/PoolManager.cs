using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //기본 표본


    [SerializeField]
    private GameObject[] prefabs;

    List<GameObject>[] pools;

    [SerializeField]
    private List<MonsterData> monsterDataList;

    public int getMonsterSize() { return prefabs.Length; }

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        int poolSize = pools.Length;

        for (int idx = 0; idx < poolSize; idx++)
        {
            pools[idx] = new List<GameObject>();
        }
    }

    public GameObject Get(int i_idx)
    {
        GameObject select = null;

        // ... 선택한 풀의 놀고 있는 겜오브젝트 접근
        foreach(GameObject item in pools[i_idx])
        {
            //비활성화 된 게임오브젝트를 확인
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                select.GetComponent<EnemyInfo>().SetMonsterData(monsterDataList[i_idx]);
                break;
            }
        }

        // .. 못찾았으면
        if (!select) //데이터가 없기때문에
        {
            // 새롭게 생성 //transform poolmanager 자식에 생성하겠다
            select = Instantiate(prefabs[i_idx], transform);
            pools[i_idx].Add(select);
            select.GetComponent<EnemyInfo>().SetMonsterData(monsterDataList[i_idx]);
        }

        return select;
    }
}
