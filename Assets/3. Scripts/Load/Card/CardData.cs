using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : ScriptableObject
{
    [SerializeField]
    private int cardIndex;
    public int iCardIndex { get { return cardIndex; } }
    public void SetCardIndex(int i_iIdx) { cardIndex = i_iIdx; }

    [SerializeField]
    private string cardName;
    public string strCardName { get { return cardName; } }
    public void SetCardName(string i_strName) { cardName = i_strName; }

    [SerializeField]
    private int gradeType;
    public int iGradeType { get { return gradeType; } }
    public void SetGradeType(int i_iGrade) { gradeType = i_iGrade; }

    [SerializeField]
    private string gradeName;
    public string strGradeName { get { return gradeName; } }
    public void SetGradeName(string i_strName) { gradeName = i_strName; }

    [SerializeField]
    private float plusPoint;
    public float fPlusPoint { get { return plusPoint; } }
    public void SetPlusPoint(float i_fPoint) { plusPoint = i_fPoint; }

    [SerializeField]
    private float dropRate;
    public float fDropRate { get { return dropRate; } }
    public void SetDropRate(float i_fRate) { dropRate = i_fRate; }

    [SerializeField]
    private int cardType;
    public int iCardType { get { return cardType; } }
    public void SetCardType(int i_iType) { cardType = i_iType; }

    [SerializeField]
    private string ScriptablePath;
    public string strScriptablePath { get { return ScriptablePath; } }
    public void SetScriptablePath(string i_strPath) { ScriptablePath = i_strPath; }
}
