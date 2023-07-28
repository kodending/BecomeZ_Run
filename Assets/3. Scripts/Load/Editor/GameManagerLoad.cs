using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Events;
using UnityEditor;

public class GameManagerLoad : MonoBehaviour
{
    string id = "1RlSXbvUTcYgrUhkd148bkApAOMa3DiiKrdmgs52z_XE";
    string sheetName = "gamemanager";

    enum GMTable
    {
        LEVEL,
        MONINDEX,
        CREATECOUNT,
        CREATETIME
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

                GMData gmData = ScriptableObject.CreateInstance<GMData>();
             
                for (int iRow = 0; iRow < rowCount; iRow++)
                {
                    //���⼭ �η��۷�������. ���� ���ĺ���.
                    ////�켱 ���� ����Ʈ�� ��´�.
                    if (iRow == (int)GMTable.LEVEL)
                        gmData.SetGMLevel(int.Parse(rw[iRow].value));

                    if (iRow == (int)GMTable.MONINDEX)
                        gmData.SetGMMonIdx(int.Parse(rw[iRow].value));

                    if (iRow == (int)GMTable.CREATECOUNT)
                        gmData.SetCreateCount(int.Parse(rw[iRow].value));

                    if (iRow == (int)GMTable.CREATETIME)
                        gmData.SetCreateTime(float.Parse(rw[iRow].value));
                }

                gmData.SetIsBossTime(gmData.fCreateTime > 0 ? false : true);

                gmData.SetScriptablePath("Assets/Resources/ScriptableData/GameManager/" +
                                                gmData.iLevel.ToString() + ".asset");

                AssetDatabase.CreateAsset(gmData, gmData.strScriptablePath);
            }
        });
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(id, sheetName), callback, mergedCells);
    }
}