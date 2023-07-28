using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Events;


//�̰� ���״�� �ߺ��� (������ ��ũ��Ʈ ���� �� �����Ұ�)
public class SheetReader : MonoBehaviour
{
    // Start is called before the first frame update

    //���� ��Ʈ�� ���̴� ID�� ã�� �������
    string id = "1RlSXbvUTcYgrUhkd148bkApAOMa3DiiKrdmgs52z_XE";
    //���� ��Ʈ �ϴܿ����̴� ��Ʈ �̸��� �ۼ��ϸ� ��
    string sheetName = "monster";

    void Start()
    {
        UpdateStats((GstuSpreadSheet ss) =>
        {
            //������ ���� �б�
            //var rw = ss.rows[ss.Cells["A1"].value];
            //foreach(var rwdat in rw)
            //{
            //    var col = ss.columns[rwdat.value];
            //    string logger = string.Empty;
            //    for (int j = 0; j < col.Count; j++)
            //    {
            //        logger += (col[j].value + ",");
            //    }
            //    Debug.Log(logger);
            //}
            //Debug.Log("---------------");

            //������ ���� �б�
            var col = ss.columns[ss.Cells["A1"].value];
            foreach (var coldat in col)
            {
                //ù��° ���� �����Ͱ��� ���� table�̱� ������ ������ȵ�
                if (coldat.Row() == 1) continue;

                var rw = ss.rows[coldat.value];
                string logger = string.Empty;
                for (int j = 0; j < rw.Count; j++)
                {
                    logger += (rw[j].value + ",");
                }
                Debug.Log(logger);
            }
            Debug.Log("---------------");
        });
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(id, sheetName), callback, mergedCells);
    }
}
