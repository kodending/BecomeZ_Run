using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    [SerializeField]
    private int healAmount;
    public int iHealAmount { get { return healAmount; } }
    public void SetHealAmount(int i_iAmount) { healAmount = i_iAmount; }

    [SerializeField]
    private string ScriptablePath;
    public string strScriptablePath { get { return ScriptablePath; } }
    public void SetScriptablePath(string i_strPath) { ScriptablePath = i_strPath; }
}
