using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private int listSize;

    [SerializeField]
    private List<CardData> cardDataList;

    [SerializeField]
    private List<Animator> gradeAnimList;

    [SerializeField]
    private List<Image> cardImgList;

    [SerializeField]
    private List<Text> cardTitleList;

    [SerializeField]
    private List<Text> cardPointList;

    [SerializeField]
    private List<Sprite> cardSpriteList;

    [SerializeField]
    private GameObject cardBG;

    [SerializeField]
    private List<GameObject> cardSelectList;

    [SerializeField]
    private List<CardData> cardRandRateList;

    enum GradeType
    {
        NORMAL,
        RARE,
        EPIC
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int idx = 1; idx < listSize + 1; idx++)
        {
            CardData cd = Resources.Load<CardData>("ScriptableData/Card/" + idx.ToString());

            cardDataList.Add(cd);
        }

        for (int idx = 0; idx < listSize;)
        {
            for (int idx2 = 0; idx2 < cardDataList[idx].fDropRate; idx2++)
            {
                cardRandRateList.Add(cardDataList[idx]);
            }

            idx++;
        }
    }

    public void OpenCard()
    {
        cardBG.SetActive(true);

        for(int idx = 0; idx < 3; idx++)
        {
            int dropIdx = Random.Range(0, cardRandRateList.Count);

            cardSelectList[idx].GetComponent<CardSelectInfo>().SetCardInfo(cardRandRateList[dropIdx]);
            CardSetting(idx, cardRandRateList[dropIdx]);
        }
    }


    void CardSetting(int idx, CardData cd)
    {
        cardTitleList[idx].text = GetCardName(cd.iCardType);

        cardPointList[idx].text = string.Format("+ {0:N2}", cd.fPlusPoint);

        CardGradeSet(idx, cd);

        cardImgList[idx].sprite = cardSpriteList[cd.iCardType];
    }

    void CardGradeSet(int idx, CardData cd)
    {
        switch (cd.iGradeType)
        {
            case (int)GradeType.NORMAL:
                gradeAnimList[idx].SetTrigger("isNormal");
                break;

            case (int)GradeType.RARE:
                gradeAnimList[idx].SetTrigger("isRare");
                break;

            case (int)GradeType.EPIC:
                gradeAnimList[idx].SetTrigger("isEpic");
                break;
        }
    }

    public void CardBGSetActive(bool i_bBool)
    {
        cardBG.SetActive(i_bBool);
    }

    public bool GetCardBGActive()
    {
        return cardBG.activeSelf;
    }

    string GetCardName(int i_cardType)
    {
        string str = null;

        switch (i_cardType)
        {
            case 0:
                str = OptionSetting.os.getString(OptionSetting.StringIndex.PLAYERSPD);
                break;

            case 1:
                str = OptionSetting.os.getString(OptionSetting.StringIndex.PLAYERHP);
                break;

            case 2:
                str = OptionSetting.os.getString(OptionSetting.StringIndex.PLAYERDEF);
                break;

            case 3:
                str = OptionSetting.os.getString(OptionSetting.StringIndex.TURRETATTK);
                break;

            case 4:
                str = OptionSetting.os.getString(OptionSetting.StringIndex.TURRETSPD);
                break;

            case 5:
                str = OptionSetting.os.getString(OptionSetting.StringIndex.TURRETRANGE);
                break;

            case 6:
                str = OptionSetting.os.getString(OptionSetting.StringIndex.ADDTURRET);
                break;
        }


        return str;
    }
}
