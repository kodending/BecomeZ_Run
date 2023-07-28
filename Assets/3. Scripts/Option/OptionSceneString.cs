using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSceneString : MonoBehaviour
{
    [SerializeField]
    Text returnTxt;

    [SerializeField]
    Text optionTxt;

    [SerializeField]
    Text lanTxt;

    [SerializeField]
    Text sndTxt;

    void Start()
    {
        RefreshTranslate();
    }

    public void RefreshTranslate()
    {
        returnTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.RETURN);
        optionTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.OPTION);
        lanTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.LANGUAGE);
        sndTxt.text = OptionSetting.os.getString(OptionSetting.StringIndex.SOUND);
    }
}
