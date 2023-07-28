using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionManager : MonoBehaviour
{
    [SerializeField]
    Text curText;

    [SerializeField]
    GameObject extendView;

    [SerializeField]
    AudioSource btnSnd;

    [SerializeField]
    List<Text> LangTexts;

    [SerializeField]
    List<Sprite> sndSprites;

    [SerializeField]
    List<Sprite> btnSprites;

    [SerializeField]
    GameObject SndBtn;

    [SerializeField]
    Transform sndImg;

    bool isMute;

    StringTableData STableData;

    [SerializeField]
    GameObject refreshString;

    [SerializeField]
    GameObject SndManager;

    enum LANGTYPE
    {
        KOR,
        ENG
    }

    // Start is called before the first frame update
    void Start()
    {
        btnSnd.Stop();
        STableData = OptionSetting.os.GetOptionData().stringTableData;
        curText.text = OptionSetting.os.getString(OptionSetting.StringIndex.COUNTRY);
        isMute = OptionSetting.os.GetIsMute();
        RefreshSoundSet();
    }

    public void OnExtendBtn()
    {
        if (!extendView.activeSelf)
        {
            extendView.SetActive(true);
            SndBtn.SetActive(false);
        }

        else
        {
            extendView.SetActive(false);
            SndBtn.SetActive(true);
        }
    }

    public void OnDisableExtendBtn()
    {
        extendView.SetActive(false);
        SndBtn.SetActive(true);
    }

    public void OnReturnBtn()
    {
        SceneManager.LoadScene("Main");
    }

    public void BtnSound()
    {
        btnSnd.Play();
    }

    public void OnEngBtn()
    {
        SelectLanguage(LANGTYPE.ENG);
        SndBtn.SetActive(true);
    }

    public void OnKorBtn()
    {
        SelectLanguage(LANGTYPE.KOR);
        SndBtn.SetActive(true);
    }

    void SelectLanguage(LANGTYPE langType)
    {
        extendView.SetActive(false);
        int langNum = 0;

        switch (langType)
        {
            case LANGTYPE.KOR:
                langNum = (int)langType;
                break;

            case LANGTYPE.ENG:
                langNum = (int)langType;
                break;
        }
        
        curText.text = LangTexts[langNum].text;

        int langIdx = langNum + 1;

        STableData = Resources.Load<StringTableData>("ScriptableData/StringTable/" + langIdx.ToString());

        SaveOption();

        refreshString.GetComponent<OptionSceneString>().RefreshTranslate();
    }

    public void OnSoundBtn()
    {
        isMute = !isMute;

        RefreshSoundSet();

        SaveOption();

        SndManager.GetComponent<SoundManager>().RefreshSound();
    }

    void RefreshSoundSet()
    {
        if (isMute)
        {
            sndImg.position = new Vector3(sndImg.position.x, SndBtn.transform.position.y + 15f, sndImg.position.z);
        }

        else
        {
            sndImg.position = new Vector3(sndImg.position.x, SndBtn.transform.position.y - 15f, sndImg.position.z);
        }

        int idx = isMute ? 1 : 0;

        sndImg.gameObject.GetComponent<Image>().sprite = sndSprites[idx];
        SndBtn.GetComponent<Image>().sprite = btnSprites[idx];
    }


    void SaveOption()
    {
        OptionData optionData = Resources.Load<OptionData>("ScriptableData/SetOption/SaveOption");

        optionData.SetStringTableData(STableData);

        optionData.SetIsMute(isMute);

        OptionSetting.os.SetOptionData(optionData);
    }
}
