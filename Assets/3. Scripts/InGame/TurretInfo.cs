using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretInfo : MonoBehaviour
{
    [SerializeField]
    private TurretData m_turretData;
    public TurretData turretData { get { return m_turretData; } }
    public void SetTurretData(TurretData i_turretData) { m_turretData = i_turretData; }

}
