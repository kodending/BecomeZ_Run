using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData : ScriptableObject
{
    [SerializeField]
    private int monsterIndex;
    public int iMonsterIndex { get { return monsterIndex; } }
    public void SetMonsterIndex(int i_iMonsterIdx) { monsterIndex = i_iMonsterIdx; }

    [SerializeField]
    private string monsterName;
    public string strMonsterName { get { return monsterName; } }
    public void SetMonsterName(string i_strName) { monsterName = i_strName; }

    [SerializeField]
    private int monsterHP;
    public int iMonsterHP { get { return monsterHP; } }
    public void SetMonsterHP(int i_iHP) { monsterHP = i_iHP; }

    [SerializeField]
    private float monsterMoveSpeed;
    public float fMonsterMoveSpeed { get { return monsterMoveSpeed; } }
    public void SetMonsterMoveSpeed(float i_fSpeed) { monsterMoveSpeed = i_fSpeed; }

    [SerializeField]
    private int monsterAttkPower;
    public int iMonsterAttkPower { get { return monsterAttkPower; } }
    public void SetMonsterAttkPower(int i_iAttkPower) { monsterAttkPower = i_iAttkPower; }

    [SerializeField]
    private float monsterAttkSpeed;
    public float fMonsterAttkSpeed { get { return monsterAttkSpeed; } }
    public void SetMonsterAttkSpeed(float i_fAttkSpeed) { monsterAttkSpeed = i_fAttkSpeed; }

    [SerializeField]
    private int HealDropRate;
    public int iHealDropRate { get { return HealDropRate; } }
    public void SetHealDropRate(int i_iDropRate) { HealDropRate = i_iDropRate; }

    [SerializeField]
    private bool isBoss;
    public bool bBoss { get { return isBoss; } }
    public void SetIsBoss(bool i_bBoss) { isBoss = i_bBoss; }

    [SerializeField]
    private int expPoint;
    public int iExpPoint { get { return expPoint; } }
    public void SetExpPoint(int i_iEXP) { expPoint = i_iEXP; }

    [SerializeField]
    private string ScriptablePath;
    public string strScriptablePath { get { return ScriptablePath; } }
    public void SetScriptablePath(string i_strPath) { ScriptablePath = i_strPath; }
}
