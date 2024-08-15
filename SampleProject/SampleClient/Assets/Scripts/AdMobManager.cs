using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Api.Mediation;
using System.Collections.Generic;

public class AdMobManager : MonoBehaviour
{
    private BannerView bannerView;
    private RewardedAd rewardedAd;
    private static AdMobManager _instance;
    private AdPosition currentAdPosition = AdPosition.Top;
    private bool areAdsDisabled;

    public static AdMobManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("AdMobManager");
                _instance = go.AddComponent<AdMobManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public AdPosition CurrentAdPosition => currentAdPosition;

    public event Action<bool> OnBannerVisibilityChanged;

    void Start()
    {
        MobileAds.Initialize(initStatus => { Debug.Log("Mobile Ads Initialized"); });
        RequestBanner();
        RequestRewardedAd();
    }

    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-8858744913757262/1212190764"; // Your banner ad unit ID
        currentAdPosition = AdPosition.Top;
        bannerView = new BannerView(adUnitId, AdSize.Banner, currentAdPosition);

        // Manually create an AdRequest
        AdRequest request = new AdRequest();
        request.Keywords.Add("unity");
        request.Keywords.Add("mobile");
        request.Keywords.Add("game");
        request.Extras.Add("example_extra_key", "example_extra_value");

        bannerView.LoadAd(request);

        bannerView.OnBannerAdLoaded += HandleBannerAdLoaded;
        bannerView.OnBannerAdLoadFailed += HandleBannerAdFailedToLoad;
    }

    private void RequestRewardedAd()
    {
        string adUnitId = "ca-app-pub-8858744913757262/5341770369"; // Your rewarded ad unit ID

        RewardedAd.Load(adUnitId, new AdRequest(), (ad, error) =>
        {
            if (error == null)
            {
                rewardedAd = ad;
                Debug.Log("Rewarded ad loaded successfully");
            }
            else
            {
                Debug.LogError("Rewarded ad failed to load: " + error);
            }
        });
    }

    public void ShowRewardedAd(Action<Reward> rewardCallback)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(rewardCallback);
        }
        else
        {
            Debug.Log("Rewarded ad is not loaded yet.");
            UIM.ShowToast(41026);
        }
    }

    public void ShowBanner()
    {
        if (bannerView != null && !areAdsDisabled)
        {
            bannerView.Show();
            OnBannerVisibilityChanged?.Invoke(true);
        }
    }

    public void HideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
            OnBannerVisibilityChanged?.Invoke(false);
        }
    }

    private void HandleBannerAdLoaded()
    {
        OnBannerVisibilityChanged?.Invoke(true);
    }

    private void HandleBannerAdFailedToLoad(LoadAdError error)
    {
        OnBannerVisibilityChanged?.Invoke(false);
    }

    public void ToggleAds()
    {
        areAdsDisabled = !areAdsDisabled;
        if (areAdsDisabled)
        {
            HideBanner();
        }
        else
        {
            ShowBanner();
        }
    }

    public void DisAbleAds()
    {
        areAdsDisabled = true;
        if (areAdsDisabled)
        {
            HideBanner();
        }
        else
        {
            ShowBanner();
        }
    }

    public bool AreAdsEnabled()
    {
        return !areAdsDisabled;
    }
}
