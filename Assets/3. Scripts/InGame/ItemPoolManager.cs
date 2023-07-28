using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    List<GameObject> pools;

    [SerializeField]
    private ItemData itemData;

    void Awake()
    {
        pools = new List<GameObject>();

        itemData = Resources.Load<ItemData>("ScriptableData/Item/" + "HPPotion");
    }

    public GameObject Get()
    {
        GameObject select = null;

        // ... 선택한 풀의 놀고 있는 겜오브젝트 접근
        foreach (GameObject item in pools)
        {
            //비활성화 된 게임오브젝트를 확인
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);

                break;
            }
        }

        // .. 못찾았으면
        if (!select) //데이터가 없기때문에
        {
            // 새롭게 생성 //transform poolmanager 자식에 생성하겠다
            select = Instantiate(prefab, transform);
            select.GetComponent<ItemFSM>().itemData = itemData;

            pools.Add(select);
        }

        return select;
    }
}