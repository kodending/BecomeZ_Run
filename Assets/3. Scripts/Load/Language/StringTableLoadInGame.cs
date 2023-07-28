using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Events;

public class StringTableLoadInGame : MonoBehaviour
{
    string id = "1RlSXbvUTcYgrUhkd148bkApAOMa3DiiKrdmgs52z_XE";
    string sheetName = "stringtable";

    enum StringTable
    {
        LOADING = 1,
        START,
        OPTION,
        EXIT,
        LANGUAGE,
        RETURN,
        GAMEOVER,
        GAMECLEAR,
        GAMEPAUSE,
        PLAY,
        RESTART,
        PLAYERSPD,
        PLAYERHP,
        PLAYERDEF,
        TURRETATTK,
        TURRETSPD,
        TURRETRANGE,
        ADDTURRET,
        SOUND,
        COUNTRY,
        CHOOSECARD
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

                int idx = coldat.Row() - 1;

                StringTableData strData = Resources.Load<StringTableData>("ScriptableData/StringTable/" + idx.ToString());

                for (int iRow = 0; iRow < rowCount; iRow++)
                {
                    //여기서 널레퍼런스난다. 여기 고쳐보자.
                    ////우선 몬스터 리스트에 담는다.
                    if (iRow == (int)StringTable.LOADING)
                        strData.SetStringLoading(rw[iRow].value);

                    if (iRow == (int)StringTable.START)
                        strData.SetStringStart(rw[iRow].value);

                    if (iRow == (int)StringTable.OPTION)
                        strData.SetStringOption(rw[iRow].value);

                    if (iRow == (int)StringTable.EXIT)
                        strData.SetStringExit(rw[iRow].value);

                    if (iRow == (int)StringTable.LANGUAGE)
                        strData.SetStringLanguage(rw[iRow].value);

                    if (iRow == (int)StringTable.RETURN)
                        strData.SetStringReturn(rw[iRow].value);

                    if (iRow == (int)StringTable.GAMEOVER)
                        strData.SetStringGameOver(rw[iRow].value);

                    if (iRow == (int)StringTable.GAMECLEAR)
                        strData.SetStringGameClear(rw[iRow].value);

                    if (iRow == (int)StringTable.GAMEPAUSE)
                        strData.SetStringGamePause(rw[iRow].value);

                    if (iRow == (int)StringTable.PLAY)
                        strData.SetStringPlay(rw[iRow].value);

                    if (iRow == (int)StringTable.RESTART)
                        strData.SetStringRestart(rw[iRow].value);

                    if (iRow == (int)StringTable.PLAYERSPD)
                        strData.SetStringPlayerSpeed(rw[iRow].value);

                    if (iRow == (int)StringTable.PLAYERHP)
                        strData.SetStringPlayerHP(rw[iRow].value);

                    if (iRow == (int)StringTable.PLAYERDEF)
                        strData.SetStringPlayerDEF(rw[iRow].value);

                    if (iRow == (int)StringTable.TURRETATTK)
                        strData.SetStringTurretAttk(rw[iRow].value);

                    if (iRow == (int)StringTable.TURRETSPD)
                        strData.SetStringTurretSpeed(rw[iRow].value);

                    if (iRow == (int)StringTable.TURRETRANGE)
                        strData.SetStringTurretRange(rw[iRow].value);

                    if (iRow == (int)StringTable.ADDTURRET)
                        strData.SetStringAddTurret(rw[iRow].value);

                    if (iRow == (int)StringTable.SOUND)
                        strData.SetStringSound(rw[iRow].value);

                    if (iRow == (int)StringTable.COUNTRY)
                        strData.SetStringCountry(rw[iRow].value);

                    if (iRow == (int)StringTable.CHOOSECARD)
                        strData.SetStringChooseCard(rw[iRow].value);
                }
            }
        });
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(id, sheetName), callback, mergedCells);
    }
}
