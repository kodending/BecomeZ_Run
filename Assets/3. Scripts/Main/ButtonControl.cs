using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    [SerializeField]
    Text UITxt;

    Vector3 vReturnPos;
    Color returnColor;

    [SerializeField]
    AudioSource btnDownSnd;

    [SerializeField]
    AudioSource btnUpSnd;

    void Start()
    {
        vReturnPos = UITxt.gameObject.transform.position;
        returnColor = UITxt.color;
        btnDownSnd.Stop();
        btnUpSnd.Stop();
    }


    public void OnPressedBtn()
    {
        btnDownSnd.Play();
        Vector3 vPos = UITxt.gameObject.transform.position;
        UITxt.gameObject.transform.position = new Vector3(vPos.x, vPos.y - 12, vPos.z);
        UITxt.color = new Color32(128, 125, 80, 255);
    }

    public void UnPressedBtn()
    {
        btnUpSnd.Play();
        UITxt.gameObject.transform.position = vReturnPos;
        UITxt.color = returnColor;
    }
}
