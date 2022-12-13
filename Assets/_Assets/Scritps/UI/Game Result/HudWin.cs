using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class HudWin : MonoBehaviour
{
    public Button btnRetry;
    public Button btnSelectStage;
    public Button btnHome;
    public Button btnNextStage;
    public Button btnWatchAds;
    public Text textNotiButtonHome;
    public GameObject[] difficultyIcons;
    public GameObject[] stars;
    public RewardElement[] rewardCells;

    private List<RewardData> winRewards = new List<RewardData>();


    public void Open(List<RewardData> rewards)
    {
        winRewards = rewards;

        gameObject.SetActive(true);

        

        for (int i = 0; i < rewardCells.Length; i++)
        {
            RewardElement cell = rewardCells[i];

            cell.gameObject.SetActive(false);
            cell.gameObject.SetActive(i < rewards.Count);

            if (i < rewards.Count)
            {
                RewardData rw = rewards[i];
                cell.SetInformation(rw);
            }
        }

       
        UIController.Instance.ActiveIngameUI(false);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_TEXT_TYPING);

        int random = UnityEngine.Random.Range(1, 101);
        btnWatchAds.gameObject.SetActive(random <= 40);
    }

    public void SelectStage()
    {
        SoundManager.Instance.PlaySfxClick();

        MainMenu.navigation = MainMenuNavigation.OpenWorldMap;
        MapChooser.navigation = WorldMapNavigation.None;
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
    }

    public void NextStage()
    {
        SoundManager.Instance.PlaySfxClick();

        MainMenu.navigation = MainMenuNavigation.OpenWorldMap;
        MapChooser.navigation = WorldMapNavigation.NextStageFromGame;
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
    }

    public void BackToMainMenu()
    {
        SoundManager.Instance.PlaySfxClick();
        UIController.Instance.BackToMainMenu();
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SoundManager.Instance.PlaySfxClick();
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_GAME_PLAY);
    }
    private void CompleteMethod(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            EventDispatcher.Instance.PostEvent(EventID.ViewAdsx2CoinEndGame, true);
            btnWatchAds.gameObject.SetActive(false);
           
            RewardUtils.Receive(winRewards);
            Popup.Instance.ShowReward(winRewards);

            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);
            FirebaseAnalyticsHelper.LogEvent("N_ViewAdsX2Reward");
        }
        else
        {
            btnWatchAds.interactable = true;
        }
    }
    public void WatchAdsX2Reward()
    {
        SoundManager.Instance.PlaySfxClick();
        btnWatchAds.interactable = false;
        bool IsRewardReady = Advertisements.Instance.IsRewardVideoAvailable();
        if(IsRewardReady)
        {
            Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
        }
       
        
        //AdMobController.Instance.ShowRewardedVideoAd(showResult =>
        //{
        //    if (showResult == ShowResult.Finished)
        //    {

        //    }
        //    else
        //    {

        //    }
        //});
    }



    
    }
    //    public void ShareImageToFacebook()
    //    {
    //        if (ProfileManager.UserProfile.countShareFacebook >= StaticValue.SHARE_FACEBOOK_LIMIT_TIMES)
    //        {
    //            Popup.Instance.Show("you have exceeded the number of times you can share today.");
    //            return;
    //        }
    //#if UNITY_EDITOR
    //        ShareFacebookSuccess();
    //#else
    //        Texture2D textureImage = imgShare.sprite.texture;

    //        FbController.Instance.ShareImage(string.Empty, textureImage, result =>
    //        {
    //            if (result)
    //            {
    //                ShareFacebookSuccess();
    //            }
    //            else
    //            {
    //                Popup.Instance.ShowToastMessage("share facebook failed");
    //            }
    //        });
    //#endif
    //    }

    //private void ShareFacebookSuccess()
    //{
    //    GameData.playerResources.ReceiveGem(StaticValue.SHARE_FACEBOOK_GEM_REWARD);
    //    SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);
    //    //btnShare.gameObject.SetActive(false);

    //    int countShare = ProfileManager.UserProfile.countShareFacebook;
    //    countShare++;
    //    ProfileManager.UserProfile.countShareFacebook.Set(countShare);

    //    Popup.Instance.Show(
    //        content: string.Format("You have received <color=yellow>{0} gems</color> for sharing", StaticValue.SHARE_FACEBOOK_GEM_REWARD),
    //        title: "congratulations");

    //    FirebaseAnalyticsHelper.LogEvent("N_ShareFacebook", "Times=" + countShare);
    //}

    

