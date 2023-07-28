using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Events;
using UnityEditor;

public class CardLoad : MonoBehaviour
{
    string id = "1RlSXbvUTcYgrUhkd148bkApAOMa3DiiKrdmgs52z_XE";
    string sheetName = "randomcard";

    enum CardTable
    {
        INDEX,
        CARDNAME,
        GRADETYPE,
        TYPENAME,
        PLUSPOINT,
        DROPRATE,
        CARDTYPE
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

                CardData cardData = ScriptableObject.CreateInstance<CardData>();

                for (int iRow = 0; iRow < rowCount; iRow++)
                {
                    //여기서 널레퍼런스난다. 여기 고쳐보자.
                    ////우선 몬스터 리스트에 담는다.
                    if (iRow == (int)CardTable.INDEX)
                        cardData.SetCardIndex(int.Parse(rw[iRow].value));

                    if (iRow == (int)CardTable.CARDNAME)
                        cardData.SetCardName(rw[iRow].value);

                    if (iRow == (int)CardTable.GRADETYPE)
                        cardData.SetGradeType(int.Parse(rw[iRow].value));

                    if (iRow == (int)CardTable.TYPENAME)
                        cardData.SetGradeName(rw[iRow].value);

                    if (iRow == (int)CardTable.PLUSPOINT)
                        cardData.SetPlusPoint(float.Parse(rw[iRow].value));

                    if (iRow == (int)CardTable.DROPRATE)
                        cardData.SetDropRate(float.Parse(rw[iRow].value));

                    if (iRow == (int)CardTable.CARDTYPE)
                        cardData.SetCardType(int.Parse(rw[iRow].value));
                }

                cardData.SetScriptablePath("Assets/Resources/ScriptableData/Card/" +
                                                cardData.iCardIndex.ToString() + ".asset");

                AssetDatabase.CreateAsset(cardData, cardData.strScriptablePath);
            }
        });
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(id, sheetName), callback, mergedCells);
    }
}
