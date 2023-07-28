using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAdmobo : MonoBehaviour
{
    private BannerView bannerView;

    public void Start()
    {
        MobileAds.Initialize(initStatus => { });
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3649238448133508/4936680024";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716"; //테스트 아이디임 아직
#else
        string adUnitId = "unexpected_platform";
#endif 

        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }

        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        this.bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        this.bannerView.LoadAd(request);
    }

    public void EndBanner()
    {
        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }
    }

    public void StartBanner()
    {
        this.RequestBanner();
    }
}
