using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

//using UnityEditor.Callbacks;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour, IInterstitialAdListener, IBannerAdListener, ISkippableVideoAdListener, IRewardedVideoAdListener
{
    public StateMachine stateMachine;
    public Button continueButton;
    public GameObject tooltip;
    public bool logging;
    public bool testing;
    public bool confirm = true;
    public string zoneId = "rewardedVideo";
    public int maxViedoAds = 3;

    private BannerView banner;
    private int videoAdCount;


    #if UNITY_EDITOR
    string appKey = "";
    

    #elif UNITY_ANDROID
    string appKey = "7d92eb14a6f22da4e8931afe1659b8cb5115b6b77629e6d4"; 
    #endif

    void Start()
    {
        banner = RequesBanner();
    }

    private BannerView RequesBanner()
    {
        #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-8810835231774698/5594118762";
        #else
        string adUnitId = "unexpected_platform";
        #endif

        BannerView banner = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder()
                            //.AddTestDevice(AdRequest.TestDeviceSimulator)
                            //.AddTestDevice("94B6F3B031BFB085513365B02FBBB6DE")
                            //.AddTestDevice("09970ED4E5B9A61393ED38E4E163783C")
        .Build();

        // Load the interstitial with the request.
        banner.LoadAd(request);
        return banner;
    }

    public void Init()
    {
        //Example for UserSettings usage
        /*
    UserSettings settings = new UserSettings();
    settings.setAge(25).setBirthday("01/01/1990").setAlcohol(UserSettings.Alcohol.NEUTRAL)
    .setSmoking(UserSettings.Smoking.NEUTRAL).setEmail("hi@appodeal.com").setFacebookId("0987654321")
    .setVkId("87654321").setGender(UserSettings.Gender.OTHER).setRelation(UserSettings.Relation.DATING)
    .setInterests("gym, cars, cinema, science").setOccupation(UserSettings.Occupation.WORK);
    */  

        Appodeal.setLogging(logging);
        Appodeal.setTesting(testing);
        if (confirm)
            Appodeal.confirm(Appodeal.SKIPPABLE_VIDEO);

        Appodeal.setBannerCallbacks(this);
        Appodeal.setInterstitialCallbacks(this);
        Appodeal.setSkippableVideoCallbacks(this);
        Appodeal.setRewardedVideoCallbacks(this);
        Appodeal.initialize(appKey, Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.SKIPPABLE_VIDEO | Appodeal.REWARDED_VIDEO);
    }

    public void showInterstitial()
    {
        Appodeal.show(Appodeal.INTERSTITIAL, "on_interstitial_button_press");
    }

    public void showSkippableVideo()
    {
        Appodeal.show(Appodeal.SKIPPABLE_VIDEO, "custom_placement");
    }

    public void showRewardedVideo()
    {
        //Appodeal.show(Appodeal.REWARDED_VIDEO, "rewarded_video");
        if (Advertisement.IsReady(zoneId))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(zoneId, options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                videoAdCount++;
                stateMachine.Continue();
                if (videoAdCount >= maxViedoAds)
                {
                    continueButton.interactable = false;
                    tooltip.SetActive(false);
                }
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    public void showBanner()
    {
        //Appodeal.show(Appodeal.BANNER_BOTTOM, "bottom_banner");
        banner.Show();
    }

    public void showInterstitialOrVideo()
    {
        Appodeal.show(Appodeal.INTERSTITIAL | Appodeal.SKIPPABLE_VIDEO);
    }

    public void hideBanner()
    {
        //Appodeal.hide(Appodeal.BANNER);
        banner.Hide();
    }

    #region Banner callback handlers

    public void onBannerLoaded()
    {
        print("Banner loaded");
    }

    public void onBannerFailedToLoad()
    {
        print("Banner failed");
    }

    public void onBannerShown()
    {
        print("Banner opened");
    }

    public void onBannerClicked()
    {
        print("banner clicked");
    }

    #endregion

    #region Interstitial callback handlers

    public void onInterstitialLoaded()
    {
        print("Interstitial loaded");
    }

    public void onInterstitialFailedToLoad()
    {
        print("Interstitial failed");
    }

    public void onInterstitialShown()
    {
        print("Interstitial opened");
    }

    public void onInterstitialClicked()
    {
        print("Interstitial clicked");
    }

    public void onInterstitialClosed()
    {
        print("Interstitial closed");
    }

    #endregion

    #region Video callback handlers

    public void onSkippableVideoLoaded()
    {
        print("Skippable Video loaded");
    }

    public void onSkippableVideoFailedToLoad()
    {
        print("Skippable Video failed");
    }

    public void onSkippableVideoShown()
    {
        print("Skippable Video opened");
    }

    public void onSkippableVideoClosed()
    {
        print("Skippable Video closed");
    }

    public void onSkippableVideoFinished()
    {
        print("Skippable Video finished");
        videoAdCount++;
        stateMachine.Continue();
        if (videoAdCount >= maxViedoAds)
        {
            continueButton.interactable = false;
            tooltip.SetActive(false);
        }
    }

    #endregion

    #region Non Skippable Video callback handlers

    public void onNonSkippableVideoLoaded()
    {
        print("NonSkippable Video loaded");
    }

    public void onNonSkippableVideoFailedToLoad()
    {
        print("NonSkippable Video failed");
    }

    public void onNonSkippableVideoShown()
    {
        print("NonSkippable Video opened");
    }

    public void onNonSkippableVideoClosed()
    {
        print("NonSkippable Video closed");
    }

    public void onNonSkippableVideoFinished()
    {
        print("NonSkippable Video finished");
        videoAdCount++;
        stateMachine.Continue();
        if (videoAdCount >= maxViedoAds)
        {
            continueButton.interactable = false;
            tooltip.SetActive(false);
        }
    }

    #endregion

    #region Rewarded Video callback handlers

    public void onRewardedVideoLoaded()
    {
        print("Rewarded Video loaded");
    }

    public void onRewardedVideoFailedToLoad()
    {
        print("Rewarded Video failed");
    }

    public void onRewardedVideoShown()
    {
        print("Rewarded Video opened");
    }

    public void onRewardedVideoClosed()
    {
        print("Rewarded Video closed");
    }

    public void ORVF()
    {
        onRewardedVideoFinished(0, "null");
    }

    public void onRewardedVideoFinished(int amount, string name)
    {
        print("Rewarded Video Reward: " + amount + " " + name);
        videoAdCount++;
        stateMachine.Continue();
        if (videoAdCount >= maxViedoAds)
        {
            continueButton.interactable = false;
            tooltip.SetActive(false);
        }
    }

    #endregion
}
