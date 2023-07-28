using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //�⺻ ǥ��


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

        // ... ������ Ǯ�� ��� �ִ� �׿�����Ʈ ����
        foreach(GameObject item in pools[i_idx])
        {
            //��Ȱ��ȭ �� ���ӿ�����Ʈ�� Ȯ��
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                select.GetComponent<EnemyInfo>().SetMonsterData(monsterDataList[i_idx]);
                break;
            }
        }

        // .. ��ã������
        if (!select) //�����Ͱ� ���⶧����
        {
            // ���Ӱ� ���� //transform poolmanager �ڽĿ� �����ϰڴ�
            select = Instantiate(prefabs[i_idx], transform);
            pools[i_idx].Add(select);
            select.GetComponent<EnemyInfo>().SetMonsterData(monsterDataList[i_idx]);
        }

        return select;
    }
}
