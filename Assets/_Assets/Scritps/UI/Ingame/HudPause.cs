using UnityEngine;
using System.Collections;
using Facebook.Unity;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class HudPause : MonoBehaviour
{
    public GameObject popupPause;
    public GameObject popupPauseCampaign;
    public GameObject popupPauseSurvival;
    public Text stageNameId;


    public void Open()
    {
        popupPause.SetActive(true);

        popupPauseCampaign.SetActive(GameData.mode == GameMode.Campaign);
        popupPauseSurvival.SetActive(GameData.mode == GameMode.Survival);

        if (GameData.mode == GameMode.Campaign)
        {
            stageNameId.text = string.Format("STAGE {0} - {1}", GameData.currentStage.id, GameData.currentStage.difficulty.ToString().ToUpper());
        }

        Pause();
    }

    public void Pause()
    {
        GameController.Instance.modeController.PauseGame();
    }

    public void Leave()
    {
        if (GameData.mode == GameMode.Campaign)
        {
            Time.timeScale = 1f;

            if (!ProfileManager.UserProfile.isRemoveAds)
            {
                bool IsInterReady = Advertisements.Instance.IsInterstitialAvailable();
                if (IsInterReady)
                {
                    Advertisements.Instance.ShowInterstitial(InterstitialClosed2);

                }
               

                //AdMobController.Instance.ShowInterstitialAd(() =>
                //{
                //    UIController.Instance.BackToMainMenu();
                //});
            }
            else
            {
                UIController.Instance.BackToMainMenu();
            }
        }
        else if (GameData.mode == GameMode.Survival)
        {
            if (AccessToken.CurrentAccessToken != null)
            {
                Time.timeScale = 1f;

                Popup.Instance.Show(
                    content: "do you really want to quit?\nyour score will be saved.",
                    type: PopupType.YesNo,
                    yesCallback: () =>
                    {
                        EventDispatcher.Instance.PostEvent(EventID.QuitSurvivalSession);
                    });
            }
            else
            {
                UIController.Instance.BackToMainMenu();
            }
        }
    }

    public void Retry()
    {
        if (GameData.mode == GameMode.Campaign)
        {
            Time.timeScale = 1f;

            if (!ProfileManager.UserProfile.isRemoveAds)
            {
                bool IsInterReady =  Advertisements.Instance.IsInterstitialAvailable();
                if(IsInterReady)
                {
                    Advertisements.Instance.ShowInterstitial(InterstitialClosed);

                }
               
            }
            else
            {
                UIController.Instance.Retry();
            }
        }
    }
    private void InterstitialClosed(string advertiser)
    {
        UIController.Instance.Retry();
        Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
    }
    private void InterstitialClosed2(string advertiser)
    {
        UIController.Instance.BackToMainMenu();
        Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
    }
    public void Resume()
    {
        GameController.Instance.modeController.ResumeGame();
        popupPause.SetActive(false);
    }
}
