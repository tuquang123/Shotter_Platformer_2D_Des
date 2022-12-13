using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyGift : MonoBehaviour
{
    [Header("DAY 1")]
    public GameObject day1_item;
    public GameObject day1_mark;

    [Header("DAY 2")]
    public GameObject day2_item;
    public GameObject day2_mark;

    [Header("DAY 3")]
    public GameObject day3_item1;
    public GameObject day3_item2;
    public GameObject day3_mark1;
    public GameObject day3_mark2;

    [Header("DAY 4")]
    public GameObject day4_item;
    public GameObject day4_mark;

    [Header("DAY 5")]
    public GameObject day5_item;
    public GameObject day5_mark;

    [Header("DAY 6")]
    public GameObject day6_item;
    public GameObject day6_mark;

    [Header("DAY 7")]
    public GameObject day7_item;
    public GameObject day7_mark;

    [Header("CONTROL")]
    public Button btnCollect;

    public static DateTime date;


    void OnEnable()
    {
        // Day 1
        day1_item.SetActive(true);

        // Day 2
        day2_item.SetActive(true);

        // Day 3
        if (ProfileManager.UserProfile.isPassFirstWeek)
        {
            day3_item1.SetActive(false);
            day3_item2.SetActive(true);
        }
        else
        {
            day3_item1.SetActive(true);
            day3_item2.SetActive(false);
        }

        // Day 4
        day4_item.SetActive(true);

        // Day 5
        day5_item.SetActive(true);

        // Day 6
        day6_item.SetActive(true);

        // Day 7
        day7_item.SetActive(true);

        switch (ProfileManager.UserProfile.getDailyGiftDay)
        {
            case 2:
                day1_mark.SetActive(true);
                break;

            case 3:
                day1_mark.SetActive(true);
                day2_mark.SetActive(true);
                break;

            case 4:
                day1_mark.SetActive(true);
                day2_mark.SetActive(true);

                if (ProfileManager.UserProfile.isPassFirstWeek == false)
                    day3_mark1.SetActive(true);
                else
                    day3_mark2.SetActive(true);

                break;

            case 5:
                day1_mark.SetActive(true);
                day2_mark.SetActive(true);

                if (ProfileManager.UserProfile.isPassFirstWeek == false)
                    day3_mark1.SetActive(true);
                else
                    day3_mark2.SetActive(true);

                day4_mark.SetActive(true);
                break;
            case 6:
                day1_mark.SetActive(true);
                day2_mark.SetActive(true);

                if (ProfileManager.UserProfile.isPassFirstWeek == false)
                    day3_mark1.SetActive(true);
                else
                    day3_mark2.SetActive(true);

                day4_mark.SetActive(true);
                day5_mark.SetActive(true);
                break;

            case 7:
                day1_mark.SetActive(true);
                day2_mark.SetActive(true);

                if (ProfileManager.UserProfile.isPassFirstWeek == false)
                    day3_mark1.SetActive(true);
                else
                    day3_mark2.SetActive(true);

                day4_mark.SetActive(true);
                day5_mark.SetActive(true);
                day6_mark.SetActive(true);
                day7_mark.SetActive(ProfileManager.UserProfile.isReceivedDailyGiftToday);
                break;
        }

        btnCollect.gameObject.SetActive(!ProfileManager.UserProfile.isReceivedDailyGiftToday);
    }

    public void Collected()
    {
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);

        int day = ProfileManager.UserProfile.getDailyGiftDay;

        switch (day)
        {
            case 1:
                GameData.playerResources.ReceiveCoin(5000);
                day1_mark.SetActive(true);
                break;

            case 2:
                GameData.playerResources.ReceiveGem(70);
                day2_mark.SetActive(true);
                break;

            case 3:
                if (day3_item1.activeInHierarchy)
                {
                    GameData.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_FAMAS);
                    day3_mark1.SetActive(true);
                }
                else if (day3_item2.activeInHierarchy)
                {
                    GameData.playerGrenades.Receive(StaticValue.GRENADE_ID_F1, 5);
                    GameData.playerResources.ReceiveCoin(5000);
                    GameData.playerBoosters.Receive(BoosterType.Hp, 2);
                    GameData.playerBoosters.Receive(BoosterType.CoinMagnet, 2);
                    GameData.playerBoosters.Receive(BoosterType.Critical, 2);
                    GameData.playerBoosters.Receive(BoosterType.Damage, 2);
                    GameData.playerBoosters.Receive(BoosterType.Speed, 2);
                    day3_mark2.SetActive(true);
                }
                break;

            case 4:
                GameData.playerResources.ReceiveTournamentTicket(5);
                day4_mark.SetActive(true);
                break;

            case 5:
                GameData.playerGrenades.Receive(StaticValue.GRENADE_ID_F1, 20);
                day5_mark.SetActive(true);
                break;

            case 6:
                GameData.playerBoosters.Receive(BoosterType.Hp, 5);
                GameData.playerBoosters.Receive(BoosterType.CoinMagnet, 5);
                GameData.playerBoosters.Receive(BoosterType.Critical, 5);
                GameData.playerBoosters.Receive(BoosterType.Damage, 5);
                GameData.playerBoosters.Receive(BoosterType.Speed, 5);
                day6_mark.SetActive(true);
                break;

            case 7:
                GameData.playerResources.ReceiveGem(200);
                GameData.playerResources.ReceiveCoin(50000);
                day7_mark.SetActive(true);
                break;
        }

        day++;

        //if (day > 7)
        //{
        //    day = 7;
        //}

        ProfileManager.UserProfile.isReceivedDailyGiftToday.Set(true);
        ProfileManager.UserProfile.getDailyGiftDay.Set(day);
        ProfileManager.UserProfile.dateGetGift.Set(date);
        ProfileManager.SaveAll();

        DebugCustom.Log("Save Date Get Gift " + date + " --- Day: " + day);

        EventDispatcher.Instance.PostEvent(EventID.ClaimDailyGift);
        btnCollect.gameObject.SetActive(false);
    }
}
