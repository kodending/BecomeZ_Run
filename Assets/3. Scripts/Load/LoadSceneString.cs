using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneString : MonoBehaviour
{
    [SerializeField]
    Text LoadTxt;

    void Start()
    {
        RefreshTranslate();
    }

    public void RefreshTranslate()
    {
        LoadTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.LOADING);
    }
}
