using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Events;
using UnityEditor;

public class MonsterLoad : MonoBehaviour
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
                //첫번째 줄은 데이터값에 대한 table이기 때문에 읽으면안됨
                if (coldat.Row() == 1) continue;

                var rw = ss.rows[coldat.value];
                int rowCount = rw.Count;

                MonsterData monsterData = ScriptableObject.CreateInstance<MonsterData>();

                for (int iRow = 0; iRow < rowCount; iRow++)
                {
                    //여기서 널레퍼런스난다. 여기 고쳐보자.
                    ////우선 몬스터 리스트에 담는다.
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

                monsterData.SetScriptablePath("Assets/Resources/ScriptableData/Monster/" + 
                                                monsterData.iMonsterIndex.ToString() + ".asset");

                AssetDatabase.CreateAsset(monsterData, monsterData.strScriptablePath);
            }
        });
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(id, sheetName), callback, mergedCells);
    }
}
