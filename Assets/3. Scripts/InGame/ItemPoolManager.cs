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

        // ... ������ Ǯ�� ��� �ִ� �׿�����Ʈ ����
        foreach (GameObject item in pools)
        {
            //��Ȱ��ȭ �� ���ӿ�����Ʈ�� Ȯ��
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);

                break;
            }
        }

        // .. ��ã������
        if (!select) //�����Ͱ� ���⶧����
        {
            // ���Ӱ� ���� //transform poolmanager �ڽĿ� �����ϰڴ�
            select = Instantiate(prefab, transform);
            select.GetComponent<ItemFSM>().itemData = itemData;

            pools.Add(select);
        }

        return select;
    }
}