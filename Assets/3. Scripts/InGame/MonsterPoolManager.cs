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

        //파일내 스크립터블 오브젝트 갯수를 확인하고
        //갯수만큼 for문돌아서 %d해서 리스트에 Add 하면될듯
        //prefabs 갯수랑 스크립터블 오브젝트 생성 갯수는 일치시켜야됨.
        //인덱스가 1부터 시작해서 1부터 해야됨

        for (int idx = 1; idx < prefabs.Length + 1; idx++)
        {
            MonsterData md = Resources.Load<MonsterData>("ScriptableData/Monster/" + idx.ToString());

            monsterDataList.Add(md);
        }
    }

    public GameObject Get(int i_idx)
    {
        GameObject select = null;

        // ... 선택한 풀의 놀고 있는 겜오브젝트 접근
        foreach (GameObject item in pools[i_idx])
        {
            //비활성화 된 게임오브젝트를 확인
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                select.GetComponent<EnemyInfo>().SetMonsterData(monsterDataList[i_idx]);
                select.GetComponent<EnemyFSM>().InitInfo();
                break;
            }
        }

        // .. 못찾았으면
        if (!select) //데이터가 없기때문에
        {
            // 새롭게 생성 //transform poolmanager 자식에 생성하겠다
            select = Instantiate(prefabs[i_idx], transform);
            select.GetComponent<EnemyInfo>().SetMonsterData(monsterDataList[i_idx]);
            pools[i_idx].Add(select);
        }

        return select;
    }
}
