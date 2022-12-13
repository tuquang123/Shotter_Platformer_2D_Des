using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyChecker : MonoBehaviour
{
    private bool alreadyCheck;

    void Start()
    {
        if (!alreadyCheck)
        {
            MasterInfo.Instance.StartGetData(false, response =>
            {
                if (response != null)
                {
                    alreadyCheck = true;
                    DateTime currentDate = new DateTime(response.data.dateTime.Year, response.data.dateTime.Month, response.data.dateTime.Day);

                    // Check new day
                    DateTime lastDateLogin = ProfileManager.UserProfile.dateLastLogin;
                    double timeCheckNewDay = TimeSpan.FromTicks(currentDate.Ticks - lastDateLogin.Ticks).TotalHours;

                    if (timeCheckNewDay >= 24)
                    {
                        ProfileManager.UserProfile.countViewAdsFreeCoin.Set(0);
                        ProfileManager.UserProfile.countShareFacebook.Set(0);
                        ProfileManager.UserProfile.countPlayTournament.Set(0);
                        ProfileManager.UserProfile.countRewardInterstitialAds.Set(0);
                        ProfileManager.UserProfile.dateLastLogin.Set(currentDate);
                        GameData.playerResources.ResetTicketNewday();

                        if (ProfileManager.UserProfile.getDailyGiftDay > 7 && ProfileManager.UserProfile.isReceivedDailyGiftToday)
                        {
                            ProfileManager.UserProfile.getDailyGiftDay.Set(1);

                            if (ProfileManager.UserProfile.isPassFirstWeek == false)
                            {
                                ProfileManager.UserProfile.isPassFirstWeek.Set(true);
                            }
                        }

                        ProfileManager.UserProfile.isReceivedDailyGiftToday.Set(false);

                        EventDispatcher.Instance.PostEvent(EventID.NewDay, currentDate);
                    }

                    EventDispatcher.Instance.PostEvent(EventID.CheckTimeNewDayDone);

                    // Check version
                    if (response.code != (int)AppStatus.Normal)
                    {
                        EventDispatcher.Instance.PostEvent(EventID.NewVersionAvailable);
                    }
                }
            });
        }
        else
        {
            EventDispatcher.Instance.PostEvent(EventID.CheckTimeNewDayDone);
        }
    }
}
