using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSetting : MonoBehaviour
{
    public static OptionSetting os;

    [SerializeField]
    OptionData curOptionData;

    public void SetOptionData(OptionData od)
    {
        curOptionData = od;
    }

    public OptionData GetOptionData() { return curOptionData; }

    public enum StringIndex
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

    void Awake()
    {
        os = this;
        curOptionData = Resources.Load<OptionData>("ScriptableData/SetOption/SaveOption");
    }

    public string getString(StringIndex idx)
    {
        string str = null;

        StringTableData curStringData = curOptionData.stringTableData;

        switch (idx)
        {
            case StringIndex.LOADING:
                str = curStringData.strLoading;
                break;

            case StringIndex.START:
                str = curStringData.strStart;
                break;

            case StringIndex.OPTION:
                str = curStringData.strOption;
                break;

            case StringIndex.EXIT:
                str = curStringData.strExit;
                break;

            case StringIndex.LANGUAGE:
                str = curStringData.strLanguage;
                break;

            case StringIndex.RETURN:
                str = curStringData.strReturn;
                break;

            case StringIndex.GAMEOVER:
                str = curStringData.strGameOver;
                break;

            case StringIndex.GAMECLEAR:
                str = curStringData.strGameClear;
                break;

            case StringIndex.GAMEPAUSE:
                str = curStringData.strGamePause;
                break;

            case StringIndex.PLAY:
                str = curStringData.strPlay;
                break;

            case StringIndex.RESTART:
                str = curStringData.strRestart;
                break;

            case StringIndex.PLAYERSPD:
                str = curStringData.strPlayerSpeed;
                break;

            case StringIndex.PLAYERHP:
                str = curStringData.strPlayerHP;
                break;

            case StringIndex.PLAYERDEF:
                str = curStringData.strPlayerDEF;
                break;

            case StringIndex.TURRETATTK:
                str = curStringData.strTurretAttk;
                break;

            case StringIndex.TURRETSPD:
                str = curStringData.strTurretSpeed;
                break;

            case StringIndex.TURRETRANGE:
                str = curStringData.strTurretRange;
                break;

            case StringIndex.ADDTURRET:
                str = curStringData.strAddTurret;
                break;

            case StringIndex.SOUND:
                str = curStringData.strSound;
                break;

            case StringIndex.COUNTRY:
                str = curStringData.strCountry;
                break;

            case StringIndex.CHOOSECARD:
                str = curStringData.strChooseCard;
                break;
        }

        return str;
    }

    public bool GetIsMute()
    {
        return curOptionData.bMute;
    }
}
