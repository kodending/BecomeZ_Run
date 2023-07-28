using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System;

public class VedioAdmobo : MonoBehaviour
{
    private InterstitialAd interstitial;

    public enum BtnType
    {
        RESTART,
        EXIT
    }

    public void RequestInterstitial(BtnType btnType)
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3649238448133508/1001304446";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910"; //테스트 아이디임 아직
#else
        string adUnitId = "unexpected_platform";
#endif 

        this.interstitial = new InterstitialAd(adUnitId);

        switch (btnType)
        {
            case BtnType.RESTART:
                this.interstitial.OnAdClosed += HandleOnAdRestartClosed;
                break;

            case BtnType.EXIT:
                this.interstitial.OnAdClosed += HandleOnAdExitClosed;
                break;
        }

        AdRequest request = new AdRequest.Builder().Build();

        this.interstitial.LoadAd(request);

        RetryGamePlay();
    }


    public void HandleOnAdRestartClosed(object sender, EventArgs args)
    {
        GameManager.gm.InitRestartInfo();
    }

    public void HandleOnAdExitClosed(object sender, EventArgs args)
    {
        GameManager.gm.InitExitInfo();
    }

    private void GameOver()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }

    public void RetryGamePlay()
    {
        GameOver();
    }
}
