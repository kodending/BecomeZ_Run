using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    [SerializeField]
    private MonsterData m_monsterData;
    public MonsterData monsterData { get { return m_monsterData; } }
    public void SetMonsterData(MonsterData i_monsterData) { m_monsterData = i_monsterData; }
}
