using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringTableData : ScriptableObject
{
    [SerializeField]
    private string loading;
    public string strLoading { get { return loading; } }
    public void SetStringLoading(string i_strName) { loading = i_strName; }

    [SerializeField]
    private string start;
    public string strStart { get { return start; } }
    public void SetStringStart(string i_strName) { start = i_strName; }

    [SerializeField]
    private string option;
    public string strOption { get { return option; } }
    public void SetStringOption(string i_strName) { option = i_strName; }

    [SerializeField]
    private string exit;
    public string strExit { get { return exit; } }
    public void SetStringExit(string i_strName) { exit = i_strName; }

    [SerializeField]
    private string language;
    public string strLanguage { get { return language; } }
    public void SetStringLanguage(string i_strName) { language = i_strName; }

    [SerializeField]
    private string m_return;
    public string strReturn { get { return m_return; } }
    public void SetStringReturn(string i_strName) { m_return = i_strName; }

    [SerializeField]
    private string gameOver;
    public string strGameOver { get { return gameOver; } }
    public void SetStringGameOver(string i_strName) { gameOver = i_strName; }

    [SerializeField]
    private string gameClear;
    public string strGameClear { get { return gameClear; } }
    public void SetStringGameClear(string i_strName) { gameClear = i_strName; }

    [SerializeField]
    private string gamePause;
    public string strGamePause { get { return gamePause; } }
    public void SetStringGamePause(string i_strName) { gamePause = i_strName; }

    [SerializeField]
    private string play;
    public string strPlay { get { return play; } }
    public void SetStringPlay(string i_strName) { play = i_strName; }

    [SerializeField]
    private string restart;
    public string strRestart { get { return restart; } }
    public void SetStringRestart(string i_strName) { restart = i_strName; }

    [SerializeField]
    private string playerSeed;
    public string strPlayerSpeed { get { return playerSeed; } }
    public void SetStringPlayerSpeed(string i_strName) { playerSeed = i_strName; }

    [SerializeField]
    private string playerHP;
    public string strPlayerHP { get { return playerHP; } }
    public void SetStringPlayerHP(string i_strName) { playerHP = i_strName; }

    [SerializeField]
    private string playerDEF;
    public string strPlayerDEF { get { return playerDEF; } }
    public void SetStringPlayerDEF(string i_strName) { playerDEF = i_strName; }

    [SerializeField]
    private string turretAttk;
    public string strTurretAttk { get { return turretAttk; } }
    public void SetStringTurretAttk(string i_strName) { turretAttk = i_strName; }

    [SerializeField]
    private string turretSpeed;
    public string strTurretSpeed { get { return turretSpeed; } }
    public void SetStringTurretSpeed(string i_strName) { turretSpeed = i_strName; }

    [SerializeField]
    private string turretRange;
    public string strTurretRange { get { return turretRange; } }
    public void SetStringTurretRange(string i_strName) { turretRange = i_strName; }

    [SerializeField]
    private string addTurret;
    public string strAddTurret { get { return addTurret; } }
    public void SetStringAddTurret(string i_strName) { addTurret = i_strName; }

    [SerializeField]
    private string sound;
    public string strSound { get { return sound; } }
    public void SetStringSound(string i_strName) { sound = i_strName; }

    [SerializeField]
    private string country;
    public string strCountry { get { return country; } }
    public void SetStringCountry(string i_strName) { country = i_strName; }

    [SerializeField]
    private string chooseCard;
    public string strChooseCard { get { return chooseCard; } }
    public void SetStringChooseCard(string i_strName) { chooseCard = i_strName; }

    [SerializeField]
    private string ScriptablePath;
    public string strScriptablePath { get { return ScriptablePath; } }
    public void SetScriptablePath(string i_strPath) { ScriptablePath = i_strPath; }
}
 