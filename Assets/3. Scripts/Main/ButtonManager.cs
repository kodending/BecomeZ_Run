using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> buttons;

    void Start()
    {
        Invoke("ActiveButton", 4);
    }

    void ActiveButton()
    {
        for (int idx = 0; idx < buttons.Count; idx++)
        {
            buttons[idx].gameObject.SetActive(true);
        }
    }

    //게임시작 버튼
    public void OnStartBtn()
    {
        SceneManager.LoadScene("InGame");
    }

    //옵션 버튼
    public void OnOptionBtn()
    {
        SceneManager.LoadScene("Option");
    }

    //나가기 버튼
    public void OnExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif //UNITY_EDITOR
    }
}
