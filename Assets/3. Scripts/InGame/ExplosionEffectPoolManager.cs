using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectPoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    List<GameObject> pools;

    void Awake()
    {
        pools = new List<GameObject>();
    }

    public GameObject Get(Vector3 i_vStartPos)
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
                StartCoroutine(select.GetComponent<ExplosionEffect>().StartEffect(i_vStartPos));

                break;
            }
        }

        // .. ��ã������
        if (!select) //�����Ͱ� ���⶧����
        {
            // ���Ӱ� ���� //transform poolmanager �ڽĿ� �����ϰڴ�
            select = Instantiate(prefab, transform);
            StartCoroutine(select.GetComponent<ExplosionEffect>().StartEffect(i_vStartPos));

            pools.Add(select);
        }

        return select;
    }
}
