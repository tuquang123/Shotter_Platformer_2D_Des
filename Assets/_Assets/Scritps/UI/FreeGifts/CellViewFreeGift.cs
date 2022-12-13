using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Advertisements;

public class CellViewFreeGift : MonoBehaviour
{
    public int times;
    public RewardElement[] rewards;
    public GameObject labelAchieved;
    public Button btnWatch;
    public Text countDown;

    private List<RewardData> _rewardData;

    void OnEnable()
    {
        if (ProfileManager.UserProfile.countViewAdsFreeCoin == times - 1)
        {
            GameData.durationNextGift -= Mathf.RoundToInt(Time.realtimeSinceStartup - GameData.timeCloseFreeGift);
            UpdateState();
        }
    }

    void OnDisable()
    {
        GameData.timeCloseFreeGift = Time.realtimeSinceStartup;
    }

    public void Init()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ViewAdsGetFreeCoin, (sender, param) => UpdateState());

        List<RewardData> rewardData = GameData.staticFreeGiftData.GetRewards(times);
        _rewardData = rewardData;

        for (int i = 0; i < rewards.Length; i++)
        {
            RewardElement element = rewards[i];

            if (i < rewardData.Count)
            {
                element.gameObject.SetActive(true);
                element.SetInformation(rewardData[i]);
            }
            else
            {
                element.gameObject.SetActive(false);
            }
        }

        UpdateState();
    }

    public void UpdateState()
    {
        int countViewAdsFreeGift = ProfileManager.UserProfile.countViewAdsFreeCoin;

        if (countViewAdsFreeGift >= times)
        {
            labelAchieved.SetActive(true);
            btnWatch.gameObject.SetActive(false);
        }
        else
        {
            labelAchieved.SetActive(false);

            if (countViewAdsFreeGift == times - 1)
            {
                if (GameData.durationNextGift > 0)
                {
                    if (gameObject.activeInHierarchy)
                    {
                        btnWatch.gameObject.SetActive(false);
                        countDown.transform.parent.gameObject.SetActive(true);

                        StopAllCoroutines();

                        StartCoroutine(StartCountDown(() =>
                        {
                            countDown.transform.parent.gameObject.SetActive(false);
                            btnWatch.gameObject.SetActive(true);
                            btnWatch.interactable = true;
                        }));
                    }
                }
                else
                {
                    countDown.transform.parent.gameObject.SetActive(false);
                    btnWatch.gameObject.SetActive(true);
                    btnWatch.interactable = true;
                }
            }
            else
            {
                btnWatch.gameObject.SetActive(true);
                btnWatch.interactable = false;
            }
        }
    }

    
    private void CompleteMethod(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            int countViewAds = ProfileManager.UserProfile.countViewAdsFreeCoin;
            countViewAds++;
            ProfileManager.UserProfile.countViewAdsFreeCoin.Set(countViewAds);

            RewardUtils.Receive(_rewardData);
            Popup.Instance.ShowReward(_rewardData);
            GameData.durationNextGift = 5;
            EventDispatcher.Instance.PostEvent(EventID.ViewAdsGetFreeCoin);
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);

            FirebaseAnalyticsHelper.LogEvent("N_GetFreeGift", times);
        }
        else
        {
            btnWatch.interactable = true;
        }
    }
    private IEnumerator StartCountDown(Action callback)
    {
        while (GameData.durationNextGift > 0)
        {
            int min = GameData.durationNextGift / 60;
            int sec = GameData.durationNextGift % 60;

            countDown.text = string.Format("{0:D2}:{1:D2}", min, sec);

            yield return StaticValue.waitOneSec;

            GameData.durationNextGift--;

            if (GameData.durationNextGift == 0)
            {
                callback();
                break;
            }
        }
    }
}
