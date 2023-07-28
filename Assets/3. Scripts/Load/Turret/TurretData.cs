using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretData : ScriptableObject
{
    [SerializeField]
    private int turretIndex;
    public int iTurretIndex { get { return turretIndex; } }
    public void SetTurretIndex(int i_iTurretIdx) { turretIndex = i_iTurretIdx; }

    [SerializeField]
    private string turretName;
    public string strTurretName { get { return turretName; } }
    public void SetTurretName(string i_strName) { turretName = i_strName; }

    [SerializeField]
    private int turretAttkPower;
    public int iTurretAttkPower { get { return turretAttkPower; } }
    public void SetTurretAttkPower(int i_iAttkPower) { turretAttkPower = i_iAttkPower; }

    [SerializeField]
    private float turretAttkSpeed;
    public float fTurretAttkSpeed { get { return turretAttkSpeed; } }
    public void SetTurretAttkSpeed(float i_fAttkSpeed) { turretAttkSpeed = i_fAttkSpeed; }

    [SerializeField]
    private float turretAttkTime;
    public float fTurretAttkTime { get { return turretAttkTime; } }
    public void SetTurretAttkTime(float i_fAttkTime) { turretAttkTime = i_fAttkTime; }

    [SerializeField]
    private string ScriptablePath;
    public string strScriptablePath { get { return ScriptablePath; } }
    public void SetScriptablePath(string i_strPath) { ScriptablePath = i_strPath; }
}
