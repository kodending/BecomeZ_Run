using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneString : MonoBehaviour
{
    [SerializeField]
    Text startTxt;

    [SerializeField]
    Text optionTxt;

    [SerializeField]
    Text exitTxt;

    void Start()
    {
        RefreshTranslate();
    }

    public void RefreshTranslate()
    {
        startTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.START);
        optionTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.OPTION);
        exitTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.EXIT);
    }
}
