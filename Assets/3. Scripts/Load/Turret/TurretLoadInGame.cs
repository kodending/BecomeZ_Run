using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Events;

public class TurretLoadInGame : MonoBehaviour
{
    string id = "1RlSXbvUTcYgrUhkd148bkApAOMa3DiiKrdmgs52z_XE";
    string sheetName = "turret";

    enum TurretTable
    {
        INDEX,
        NAME,
        ATTACKPOWER,
        ATTACKSPEED,
        ATTACKTIME
    };

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

                int curIndex = coldat.Row() - 1;

                TurretData turretData = Resources.Load<TurretData>("ScriptableData/Turret/" + curIndex.ToString());

                for (int iRow = 0; iRow < rowCount; iRow++)
                {
                    //여기서 널레퍼런스난다. 여기 고쳐보자.
                    ////우선 몬스터 리스트에 담는다.
                    if (iRow == (int)TurretTable.INDEX)
                        turretData.SetTurretIndex(int.Parse(rw[iRow].value));

                    if (iRow == (int)TurretTable.NAME)
                        turretData.SetTurretName(rw[iRow].value);

                    if (iRow == (int)TurretTable.ATTACKPOWER)
                        turretData.SetTurretAttkPower(int.Parse(rw[iRow].value));

                    if (iRow == (int)TurretTable.ATTACKSPEED)
                        turretData.SetTurretAttkSpeed(float.Parse(rw[iRow].value));

                    if (iRow == (int)TurretTable.ATTACKTIME)
                        turretData.SetTurretAttkTime(float.Parse(rw[iRow].value));

                }
            }
        });
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(id, sheetName), callback, mergedCells);
    }
}
