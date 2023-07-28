using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour
{
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

        //���ϳ� ��ũ���ͺ� ������Ʈ ������ Ȯ���ϰ�
        //������ŭ for�����Ƽ� %d�ؼ� ����Ʈ�� Add �ϸ�ɵ�
        //prefabs ������ ��ũ���ͺ� ������Ʈ ���� ������ ��ġ���Ѿߵ�.
        //�ε����� 1���� �����ؼ� 1���� �ؾߵ�

        for (int idx = 1; idx < prefabs.Length + 1; idx++)
        {
            MonsterData md = Resources.Load<MonsterData>("ScriptableData/Monster/" + idx.ToString());

            monsterDataList.Add(md);
        }
    }

    public GameObject Get(int i_idx)
    {
        GameObject select = null;

        // ... ������ Ǯ�� ��� �ִ� �׿�����Ʈ ����
        foreach (GameObject item in pools[i_idx])
        {
            //��Ȱ��ȭ �� ���ӿ�����Ʈ�� Ȯ��
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                select.GetComponent<EnemyInfo>().SetMonsterData(monsterDataList[i_idx]);
                select.GetComponent<EnemyFSM>().InitInfo();
                break;
            }
        }

        // .. ��ã������
        if (!select) //�����Ͱ� ���⶧����
        {
            // ���Ӱ� ���� //transform poolmanager �ڽĿ� �����ϰڴ�
            select = Instantiate(prefabs[i_idx], transform);
            select.GetComponent<EnemyInfo>().SetMonsterData(monsterDataList[i_idx]);
            pools[i_idx].Add(select);
        }

        return select;
    }
}
