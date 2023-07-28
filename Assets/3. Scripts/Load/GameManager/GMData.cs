using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMData : ScriptableObject
{
    [SerializeField]
    private int GMLevel;
    public int iLevel { get { return GMLevel; } }
    public void SetGMLevel(int i_iLevel) { GMLevel = i_iLevel; }

    [SerializeField]
    private int GMMonIdx;
    public int iMonIdx { get { return GMMonIdx; } }
    public void SetGMMonIdx(int i_idx) { GMMonIdx = i_idx; }

    [SerializeField]
    private int GMCount;
    public int iCreateCount { get { return GMCount; } }
    public void SetCreateCount(int i_idx) { GMCount = i_idx; }

    [SerializeField]
    private float GMTime;
    public float fCreateTime { get { return GMTime; } }
    public void SetCreateTime(float i_fTime) { GMTime = i_fTime; }

    [SerializeField]
    private bool isBoss;
    public bool bBossTime { get { return isBoss; } }
    public void SetIsBossTime(bool i_bBoss) { isBoss = i_bBoss; }

    [SerializeField]
    private string ScriptablePath;
    public string strScriptablePath { get { return ScriptablePath; } }
    public void SetScriptablePath(string i_strPath) { ScriptablePath = i_strPath; }
}
