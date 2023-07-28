using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnBtnControl : MonoBehaviour
{
    [SerializeField]
    Text UITxt;

    Vector3 vReturnPos;
    Color returnColor;

    void Start()
    {
        vReturnPos = UITxt.gameObject.transform.position;
        returnColor = UITxt.color;
    }

    public void OnPressedBtn()
    {
        Vector3 vPos = UITxt.gameObject.transform.position;
        UITxt.gameObject.transform.position = new Vector3(vPos.x, vPos.y - 20, vPos.z);
        UITxt.color = new Color32(128, 125, 80, 255);
    }

    public void UnPressedBtn()
    {
        UITxt.gameObject.transform.position = vReturnPos;
        UITxt.color = returnColor;
    }
}
