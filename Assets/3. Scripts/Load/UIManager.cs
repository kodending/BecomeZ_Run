using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> logos;

    [SerializeField]
    Image backImg;

    [SerializeField]
    GameObject loadUI;

    bool isOnLoad;
    float fCurTime;

    void Start()
    {
        isOnLoad = false;
        Invoke("DisableLogo", 3);
    }

    void DisableLogo()
    {
        for(int idx = 0; idx < logos.Count; idx++)
        {
            logos[idx].gameObject.SetActive(false);
        }

        backImg.color = new Color32(0, 0, 0, 255);

        Invoke("OnLoadUI", 0.5f);
    }

    void Update()
    {
        if (isOnLoad)
        {
            fCurTime += Time.deltaTime;

            if(fCurTime >= 3.0f)
            {
                fCurTime = 0;
                isOnLoad = false;
                SceneManager.LoadScene("Main");
            }
        }
            
    }

    void OnLoadUI()
    {
        isOnLoad = true;
        loadUI.SetActive(true);
    }
}
