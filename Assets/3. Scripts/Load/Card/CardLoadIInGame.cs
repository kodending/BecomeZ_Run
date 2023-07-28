using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Events;

public class CardLoadIInGame : MonoBehaviour
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
                //ù��° ���� �����Ͱ��� ���� table�̱� ������ ������ȵ�
                if (coldat.Row() == 1) continue;

                var rw = ss.rows[coldat.value];
                int rowCount = rw.Count;

                int curIndex = coldat.Row() - 1;

                CardData cardData = Resources.Load<CardData>("ScriptableData/Card/" + curIndex.ToString());

                for (int iRow = 0; iRow < rowCount; iRow++)
                {
                    //���⼭ �η��۷�������. ���� ���ĺ���.
                    ////�켱 ���� ����Ʈ�� ��´�.
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
            }
        });
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(id, sheetName), callback, mergedCells);
    }
}
