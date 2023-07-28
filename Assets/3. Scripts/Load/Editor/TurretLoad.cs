using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Events;
using UnityEditor;

public class TurretLoad : MonoBehaviour
{
    // Start is called before the first frame update

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
                //ù��° ���� �����Ͱ��� ���� table�̱� ������ ������ȵ�
                if (coldat.Row() == 1) continue;

                var rw = ss.rows[coldat.value];
                int rowCount = rw.Count;

                TurretData turretData = ScriptableObject.CreateInstance<TurretData>();

                for (int iRow = 0; iRow < rowCount; iRow++)
                {
                    //���⼭ �η��۷�������. ���� ���ĺ���.
                    ////�켱 ���� ����Ʈ�� ��´�.
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

                turretData.SetScriptablePath("Assets/Resources/ScriptableData/Turret/" +
                                                turretData.iTurretIndex.ToString() + ".asset");

                AssetDatabase.CreateAsset(turretData, turretData.strScriptablePath);
            }
        });
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(id, sheetName), callback, mergedCells);
    }
}
