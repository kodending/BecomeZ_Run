using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionData : ScriptableObject
{
    [SerializeField]
    private StringTableData stData;
    public StringTableData stringTableData { get { return stData; } }
    public void SetStringTableData(StringTableData i_data) { stData = i_data; }

    [SerializeField]
    private bool mute;
    public bool bMute { get { return mute; } }
    public void SetIsMute(bool i_bMute) { mute = i_bMute; }

    [SerializeField]
    private string ScriptablePath;
    public string strScriptablePath { get { return ScriptablePath; } }
    public void SetScriptablePath(string i_strPath) { ScriptablePath = i_strPath; }
}
