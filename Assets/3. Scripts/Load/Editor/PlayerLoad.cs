using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Events;
using UnityEditor;

public class PlayerLoad : MonoBehaviour
{
    string id = "1RlSXbvUTcYgrUhkd148bkApAOMa3DiiKrdmgs52z_XE";
    string sheetName = "player";

    enum PlayerTable
    {
        SPEED,
        HEALTH,
        DEFENSE,
        STARTEXP,
        NEXTEXP
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

                PlayerData playerData = ScriptableObject.CreateInstance<PlayerData>();

                for (int iRow = 0; iRow < rowCount; iRow++)
                {
                    //여기서 널레퍼런스난다. 여기 고쳐보자.
                    ////우선 몬스터 리스트에 담는다.
                    if (iRow == (int)PlayerTable.SPEED)
                        playerData.SetMoveSpeed(float.Parse(rw[iRow].value));

                    if (iRow == (int)PlayerTable.HEALTH)
                        playerData.SetHP(int.Parse(rw[iRow].value));

                    if (iRow == (int)PlayerTable.DEFENSE)
                        playerData.SetDefense(int.Parse(rw[iRow].value));

                    if (iRow == (int)PlayerTable.STARTEXP)
                        playerData.SetStartEXP(int.Parse(rw[iRow].value));

                    if (iRow == (int)PlayerTable.NEXTEXP)
                        playerData.SetNextEXP(float.Parse(rw[iRow].value));

                }

                playerData.SetScriptablePath("Assets/Resources/ScriptableData/Player/" +
                                                "player" + ".asset");

                AssetDatabase.CreateAsset(playerData, playerData.strScriptablePath);
            }
        });
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(id, sheetName), callback, mergedCells);
    }
}
