using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSceneString : MonoBehaviour
{
    [SerializeField]
    Text selectCardTxt;

    [SerializeField]
    Text pauseTxt;

    [SerializeField]
    Text playTxt;

    [SerializeField]
    Text restartTxt;

    [SerializeField]
    Text exitTxt;

    void Start()
    {
        RefreshTranslate();
    }

    public void RefreshTranslate()
    {
        selectCardTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.CHOOSECARD);
        pauseTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.GAMEPAUSE);
        playTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.PLAY);
        restartTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.RESTART);
        exitTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.EXIT);
    }
}
