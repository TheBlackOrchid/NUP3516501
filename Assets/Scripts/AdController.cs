using System;
using UnityEngine;
using UnityEngine.UI;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class AdController : MonoBehaviour
{
    #if UNITY_EDITOR
    string appKey = "";
    

    #elif UNITY_ANDROID
    string appKey = "7d92eb14a6f22da4e8931afe1659b8cb5115b6b77629e6d4";
    #endif

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

//        Appodeal.setLogging(true);
//        Appodeal.setTesting(true);
//        Appodeal.confirm(Appodeal.SKIPPABLE_VIDEO);
//
//        Appodeal.setBannerCallbacks(this);
//        Appodeal.setInterstitialCallbacks(this);
//        Appodeal.setSkippableVideoCallbacks(this);
//        Appodeal.setRewardedVideoCallbacks(this);
//        Appodeal.initialize(appKey, Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.SKIPPABLE_VIDEO | Appodeal.REWARDED_VIDEO);
    }

}
