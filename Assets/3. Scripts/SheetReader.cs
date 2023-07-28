using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Events;


//이건 말그대로 견본임 (데이터 스크립트 만들 때 참고할것)
public class SheetReader : MonoBehaviour
{
    // Start is called before the first frame update

    //구글 시트에 보이는 ID를 찾아 넣으면됨
    string id = "1RlSXbvUTcYgrUhkd148bkApAOMa3DiiKrdmgs52z_XE";
    //구글 시트 하단에보이는 시트 이름을 작성하면 됨
    string sheetName = "monster";

    void Start()
    {
        UpdateStats((GstuSpreadSheet ss) =>
        {
            //세로줄 먼저 읽기
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

            //가로줄 먼저 읽기
            var col = ss.columns[ss.Cells["A1"].value];
            foreach (var coldat in col)
            {
                //첫번째 줄은 데이터값에 대한 table이기 때문에 읽으면안됨
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
