using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;

    List<GameObject>[] pools;

    [SerializeField]
    private List<TurretData> turretDataList;

    public int GetTurretSize() { return prefabs.Length; }

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

        for(int idx = 1; idx < prefabs.Length + 1; idx++)
        {
            TurretData td = Resources.Load<TurretData>("ScriptableData/Turret/" + idx.ToString());

            turretDataList.Add(td);
        }
    }

    public GameObject Get(int i_idx)
    {
        GameObject select = null;

        foreach (GameObject item in pools[i_idx])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                select.GetComponent<TurretInfo>().SetTurretData(turretDataList[i_idx]);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[i_idx], transform);
            select.GetComponent<TurretInfo>().SetTurretData(turretDataList[i_idx]);
            pools[i_idx].Add(select);
        }

        return select;
    }
}
