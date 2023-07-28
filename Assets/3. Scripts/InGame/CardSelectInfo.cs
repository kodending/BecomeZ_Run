using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectInfo : MonoBehaviour
{
    public CardData cardData;

    public void SetCardInfo(CardData cd)
    {
        cardData = cd;
    }

    public void OnClickButton()
    {
        GameManager.gm.InputCardInfo(cardData);
    }
}
