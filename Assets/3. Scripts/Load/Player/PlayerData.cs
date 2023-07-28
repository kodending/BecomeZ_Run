using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : ScriptableObject
{
    [SerializeField]
    private float moveSpeed;
    public float fMoveSpeed { get { return moveSpeed; } }
    public void SetMoveSpeed(float i_fSpeed) { moveSpeed = i_fSpeed; }

    [SerializeField]
    private int HP;
    public int iHP { get { return HP; } }
    public void SetHP (int i_iHP) { HP = i_iHP; }

    [SerializeField]
    private int defense;
    public int iDefense { get { return defense; } }
    public void SetDefense(int i_iDf) { defense = i_iDf; }

    [SerializeField]
    private int startEXP;
    public int iStartEXP { get { return startEXP; } }
    public void SetStartEXP(int i_iEXP) { startEXP = i_iEXP; }

    [SerializeField]
    private float nextEXP;
    public float fNextEXP { get { return nextEXP; } }
    public void SetNextEXP(float i_fEXP) { nextEXP = i_fEXP; }

    [SerializeField]
    private string ScriptablePath;
    public string strScriptablePath { get { return ScriptablePath; } }
    public void SetScriptablePath(string i_strPath) { ScriptablePath = i_strPath; }
}
