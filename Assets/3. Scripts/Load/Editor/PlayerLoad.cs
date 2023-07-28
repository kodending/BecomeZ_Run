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
                //ù��° ���� �����Ͱ��� ���� table�̱� ������ ������ȵ�
                if (coldat.Row() == 1) continue;

                var rw = ss.rows[coldat.value];
                int rowCount = rw.Count;

                PlayerData playerData = ScriptableObject.CreateInstance<PlayerData>();

                for (int iRow = 0; iRow < rowCount; iRow++)
                {
                    //���⼭ �η��۷�������. ���� ���ĺ���.
                    ////�켱 ���� ����Ʈ�� ��´�.
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
