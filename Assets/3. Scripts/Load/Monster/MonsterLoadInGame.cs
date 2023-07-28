using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Events;

public class MonsterLoadInGame : MonoBehaviour
{
    string id = "1RlSXbvUTcYgrUhkd148bkApAOMa3DiiKrdmgs52z_XE";
    string sheetName = "monster";

    enum MonsterTable
    {
        INDEX,
        NAME,
        HEALTH,
        MOVESPEED,
        ATTKPOWER,
        ATTKSPEED,
        HEALDROPRATE,
        ISBOSS,
        EXPPOINT
    }

    void Start()
    {
        UpdateStats((GstuSpreadSheet ss) =>
        {
            var col = ss.columns[ss.Cells["A1"].value];
            foreach (var coldat in col)
            {
                //ù��° ���� �����Ͱ��� ���� table�̱� ������ ������ȵ�
                if (coldat.Row() == 1) continue;

                var rw = ss.rows[coldat.value];
                int rowCount = rw.Count;

                int curIndex = coldat.Row() - 1;

                MonsterData monsterData = Resources.Load<MonsterData>("ScriptableData/Monster/" + curIndex.ToString());

                for (int iRow = 0; iRow < rowCount; iRow++)
                {
                    //���⼭ �η��۷�������. ���� ���ĺ���.
                    ////�켱 ���� ����Ʈ�� ��´�.
                    if (iRow == (int)MonsterTable.INDEX)
                        monsterData.SetMonsterIndex(int.Parse(rw[iRow].value));

                    if (iRow == (int)MonsterTable.NAME)
                        monsterData.SetMonsterName(rw[iRow].value);

                    if (iRow == (int)MonsterTable.HEALTH)
                        monsterData.SetMonsterHP(int.Parse(rw[iRow].value));

                    if (iRow == (int)MonsterTable.MOVESPEED)
                        monsterData.SetMonsterMoveSpeed(float.Parse(rw[iRow].value));

                    if (iRow == (int)MonsterTable.ATTKPOWER)
                        monsterData.SetMonsterAttkPower(int.Parse(rw[iRow].value));

                    if (iRow == (int)MonsterTable.ATTKSPEED)
                        monsterData.SetMonsterAttkSpeed(float.Parse(rw[iRow].value));

                    if (iRow == (int)MonsterTable.HEALDROPRATE)
                        monsterData.SetHealDropRate(int.Parse(rw[iRow].value));

                    if (iRow == (int)MonsterTable.ISBOSS)
                        monsterData.SetIsBoss(int.Parse(rw[iRow].value) > 0 ? true : false);

                    if (iRow == (int)MonsterTable.EXPPOINT)
                        monsterData.SetExpPoint(int.Parse(rw[iRow].value));
                }
            }
        });
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(id, sheetName), callback, mergedCells);
    }
}
