using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Grounds;

    private int iMaxCount;

    [SerializeField]
    private float fMoveSpeed;

    [SerializeField]
    private Transform ReturnBoxPos;

    void Awake()
    {
        iMaxCount = Grounds.Count;
    }

    // Update is called once per frame
    void Update()
    {
        for (int idx = 0; idx < iMaxCount; idx++)
        {
            Vector3 curPos = Grounds[idx].transform.position;

            Grounds[idx].transform.position = new Vector3(curPos.x - fMoveSpeed * Time.deltaTime, curPos.y, curPos.z);

            CheckPos(idx);
        }

    }

    void CheckPos(int i_idx)
    {
        if(ReturnBoxPos.transform.position.x >= Grounds[i_idx].transform.position.x)
        {
            int returnIdx = 0;

            switch (i_idx)
            {
                case 0:
                    returnIdx = 2;
                break;
                    
                case 1:
                    returnIdx = 0;
                break;

                case 2:
                    returnIdx = 1;
                break;
            }

            Vector3 vPos = Grounds[returnIdx].transform.position;
            Grounds[i_idx].transform.position = new Vector3(vPos.x + 150, vPos.y, vPos.z);
        }
    }
}
