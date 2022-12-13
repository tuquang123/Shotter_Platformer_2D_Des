using System;
using System.Collections.Generic;
using UnityEngine.Advertisements;

public class AdUtils
{
    //private static int rewardShowAdmobCount = 0;
    //private static int videoShowAdmobCount = 0;

    public static void ShowRewardedAds(Action<ShowResult> callback)
    {
#if UNITY_EDITOR
        if (callback != null)
            callback(ShowResult.Finished);
#else
        AdMobController.Instance.ShowRewardedVideoAd(callback);
#endif

        //if (rewardShowAdmobCount >= 3)
        //{
        //    rewardShowAdmobCount = 0;
        //    UnityAdsController.ShowRewardAds(callback);
        //}
        //else
        //{
        //    rewardShowAdmobCount++;
        //    AdMobController.Instance.ShowRewardedVideoAd(callback);
        //}
    }

    public static void ShowVideoAds()
    {if (Advertisements.Instance.IsRewardVideoAvailable())
        {
            Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
        }
    
        //AdMobController.Instance.ShowRewardedVideoAd(null);

            //if (videoShowAdmobCount >= 3)
            //{
            //    videoShowAdmobCount = 0;
            //    UnityAdsController.ShowVideoAds();
            //}
            //else
            //{
            //    videoShowAdmobCount++;
            //    AdMobController.Instance.ShowRewardedVideoAd(null);
            //}
    }
    private static void CompleteMethod(bool completed, string advertiser)
    {
        if (completed == true)
        {
            //give the reward
        }
        else
        {
            //no reward
        }
    }

    public static void ShowInterstitialAds()
    {
        Advertisements.Instance.ShowInterstitial();
    }
}
