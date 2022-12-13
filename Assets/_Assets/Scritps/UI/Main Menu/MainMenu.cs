using Facebook.Unity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private enum SceneMenu
    {
        Main,
        Campaign,
        Soldier,
        Weapons,
        Setting,
        Quests,
        Tournament,
        SkillTree,
    }

    public static MainMenuNavigation navigation;

    public GameObject panelMainMenu;
    public GameObject panelSoldier;
    public GameObject panelWeapon;
    public GameObject panelSelectStage;
    public GameObject panelQuests;
    public GameObject panelIap;
    public GameObject panelSkillTree;

    [Header("DAILY GIFT")]
    public GameObject panelDailyGift;
    public Button btnShowDailyGift;

    [Header("FREE GIFT")]
    public FreeGiftController freeGiftController;

    [Header("SKILL")]
    public GameObject btnSkill;

    [Header("SPECIAL OFFER")]
    public Button btnStarterPack;
    public GameObject panelStarterPack;
    public SpecialOfferController specialOfferController;

    [Header("NOTIFICATION")]
    public GameObject notiWeapon;
    public GameObject notiSoldier;
    public GameObject notiDailyGift;
    public Text notiQuests;
    public GameObject notiTournament;
    public Text numberUnusedSkillPoints;

    [Header("TOURNAMENT")]
    public HudTournamentRanking hudTournament;
    public GameObject popupLoginFacebook;

    [Space(15f)]
    public Animation[] starterAnims;
    [SerializeField]
    private SceneMenu currentScene = SceneMenu.Main;
    private bool isWaitingShowDailyGift;
    private bool isNewVersionAvailable;


    #region UNITY METHOD

    void Awake()
    {
        // Khong duoc cho xuong Start khong thi loi Daily Gift
        Time.timeScale = 1f;

        EventDispatcher.Instance.RegisterListener(EventID.CheckTimeNewDayDone, (sender, param) => OnCheckTimeNewDayDone());
        EventDispatcher.Instance.RegisterListener(EventID.ViewAdsGetFreeCoin, (sender, param) => CheckAllNotifications());
        EventDispatcher.Instance.RegisterListener(EventID.ClaimDailyGift, (sender, param) => CheckAllNotifications());
        EventDispatcher.Instance.RegisterListener(EventID.BuySpecialOffer, (sender, param) => CheckAllNotifications());
        EventDispatcher.Instance.RegisterListener(EventID.ClickStartTournament, (sender, param) => StartChallenge((int)param));

        EventDispatcher.Instance.RegisterListener(EventID.BuyStarterPack, (sender, param) =>
        {
            CheckAllNotifications();
            ShowStarterPack(false);
            btnStarterPack.gameObject.SetActive(false);
        });

        EventDispatcher.Instance.RegisterListener(EventID.NewDay, (sender, param) =>
        {
            DateTime serverTime = param != null ? (DateTime)param : StaticValue.defaultDate;
            DailyGift.date = serverTime;

            if (currentScene == SceneMenu.Main)
            {
                ShowDailyGift();
            }
            else
            {
                isWaitingShowDailyGift = true;
            }

            DebugCustom.Log("Receive Event Daily Gift: " + serverTime.ToString());
        });

        EventDispatcher.Instance.RegisterListener(EventID.NewVersionAvailable, (sender, param) =>
        {
            isNewVersionAvailable = true;
        });
    }

    void Start()
    {
        SoundManager.Instance.StopSfx();
        SoundManager.Instance.PlayMusic(StaticValue.SOUND_MUSIC_MENU);

        switch (navigation)
        {
            case MainMenuNavigation.OpenWorldMap:
                ShowCampaign();
                break;
            case MainMenuNavigation.ShowUpgradeWeapon:
                ShowWeapon();
                break;
            case MainMenuNavigation.ShowUpgradeSoldier:
                ShowSoldiers();
                break;
            case MainMenuNavigation.ShowUpgradeSkill:
                ShowSkillTree();
                break;
        }

        navigation = MainMenuNavigation.None;
        freeGiftController.Init();

        CheckAllNotifications();

        for (int i = 0; i < starterAnims.Length; i++)
        {
            starterAnims[i].Play();
        }

        btnStarterPack.gameObject.SetActive(!ProfileManager.UserProfile.isPurchasedStarterPack);
        btnSkill.gameObject.SetActive(GameData.playerTutorials.IsCompletedStep(TutorialType.Character));

        ShowRecommends();

        // Auto backup
        Popup.Instance.setting.AutoBackupData();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Back();
        }
    }

    #endregion


    #region HEADER

    public void Back()
    {
        if (GameData.isShowingTutorial)
            return;

        SoundManager.Instance.PlaySfxClick();

        if (popupLoginFacebook.activeSelf)
        {
            popupLoginFacebook.SetActive(false);
            return;
        }

        if (Popup.Instance.IsInstantLoading)
        {
            return;
        }

        if (Popup.Instance.IsShowing)
        {
            Popup.Instance.Hide();
            return;
        }

        if (panelIap.activeSelf)
        {
            switch (currentScene)
            {
                case SceneMenu.Campaign:
                    panelSelectStage.SetActive(true);
                    break;
                case SceneMenu.Soldier:
                    panelSoldier.SetActive(true);
                    break;
                case SceneMenu.Weapons:
                    panelWeapon.SetActive(true);
                    break;
                case SceneMenu.Setting:
                    ShowMainMenu();
                    break;
                case SceneMenu.Main:
                    ShowMainMenu();
                    break;
                case SceneMenu.Quests:
                    panelQuests.SetActive(true);
                    break;
                case SceneMenu.SkillTree:
                    ShowSkillTree();
                    break;
            }

            panelIap.SetActive(false);
            return;
        }

        if (panelSkillTree.activeSelf)
        {
            panelSkillTree.SetActive(false);
            ShowSoldiers();
            return;
        }

        if (panelDailyGift.activeSelf)
        {
            HideDailyGift();
            return;
        }

        if (freeGiftController.gameObject.activeSelf)
        {
            freeGiftController.Close();
            return;
        }

        if (panelSoldier.activeSelf)
        {
            panelSoldier.SetActive(false);
            ShowMainMenu();
            return;
        }

        if (panelQuests.activeSelf)
        {
            panelQuests.SetActive(false);
            ShowMainMenu();
            return;
        }

        if (panelWeapon.activeSelf)
        {
            panelWeapon.SetActive(false);
            ShowMainMenu();
            return;
        }

        if (panelSelectStage.activeSelf)
        {
            panelSelectStage.SetActive(false);
            ShowMainMenu();
            return;
        }

        if (hudTournament.popupRank.activeSelf)
        {
            hudTournament.Close();
            ShowMainMenu();
            return;
        }

        if (panelMainMenu.activeSelf)
        {
            Popup.Instance.Show(
                content: "do you want to exit game?",
                title: "confirmation",
                type: PopupType.YesNo,
                yesCallback: () =>
                {
#if UNITY_EDITOR
                    SceneManager.LoadScene(StaticValue.SCENE_LOGIN);
#else
                    Application.Quit();
#endif
                });

            return;
        }
    }

    public void ShowBuyCoinPack()
    {
        panelMainMenu.SetActive(false);
        panelSoldier.SetActive(false);
        panelWeapon.SetActive(false);
        panelSelectStage.SetActive(false);
        panelDailyGift.SetActive(false);
        panelSkillTree.SetActive(false);
        panelIap.SetActive(true);

        ShopIAP.Instance.OpenCoinShop();
        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowBuyGemPack()
    {
        panelMainMenu.SetActive(false);
        panelSoldier.SetActive(false);
        panelWeapon.SetActive(false);
        panelSelectStage.SetActive(false);
        panelDailyGift.SetActive(false);
        panelSkillTree.SetActive(false);
        panelIap.SetActive(true);

        ShopIAP.Instance.OpenGemShop();
        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowBuyIapPacks()
    {
        panelMainMenu.SetActive(false);
        panelSoldier.SetActive(false);
        panelWeapon.SetActive(false);
        panelSelectStage.SetActive(false);
        panelDailyGift.SetActive(false);
        panelSkillTree.SetActive(false);
        panelIap.SetActive(true);

        ShopIAP.Instance.OpenEssentialShop();

        SoundManager.Instance.PlaySfxClick();

        //FirebaseAnalyticsHelper.LogEvent("EnterStarterPack");
    }

    public void ShowSetting()
    {
        Popup.Instance.setting.Show();
        SoundManager.Instance.PlaySfxClick();
        DebugCustom.Log("ShowSetting");

    }

    #endregion


    #region MAIN MENU

    public void ShowStarterPack(bool isShow)
    {
        panelStarterPack.SetActive(isShow);

        if (isShow)
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_SHOW_DIALOG);
    }

    public void ShowSkillTree()
    {
        currentScene = SceneMenu.SkillTree;

        panelSoldier.SetActive(false);
        panelSkillTree.SetActive(true);

        SoundManager.Instance.PlaySfxClick();

        if (GameData.isShowingTutorial)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepEnterSkillTree);
        }
    }

    public void ShowTournament()
    {
        SoundManager.Instance.PlaySfxClick();

        //if (IsAvailablePlayTournament() == false)
        //{
        //    string msg = "you need to complete stage 1.3 (normal) to join tournament";
        //    Popup.Instance.ShowToastMessage(msg, ToastLength.Long);
        //    return;
        //}

        Popup.Instance.ShowInstantLoading();

        MasterInfo.Instance.StartGetData(false, res =>
        {
            if (res != null)
            {
                if (FB.IsLoggedIn)
                {
                    if (FireBaseDatabase.Instance.IsAuthenticated)
                    {
                        FireBaseDatabase.Instance.GetTopTournament(50, MasterInfo.Instance.GetCurrentWeekRangeString(), data =>
                        {
                            FillTournamentData(data);
                        });
                    }
                    else
                    {
                        FireBaseDatabase.Instance.AuthenWithFacebook(AccessToken.CurrentAccessToken.UserId, AccessToken.CurrentAccessToken.TokenString, AuthenFirebaseCallback);
                    }
                }
                else
                {
                    Popup.Instance.HideInstantLoading();
                    popupLoginFacebook.SetActive(true);
                }
            }
            else
            {
                Popup.Instance.HideInstantLoading();
                Popup.Instance.ShowToastMessage("Fetching data failed", ToastLength.Long);
            }
        });
    }

    public void ShowSoldiers()
    {
        currentScene = SceneMenu.Soldier;

        panelMainMenu.SetActive(false);
        panelSoldier.SetActive(true);
        panelWeapon.SetActive(false);
        panelSelectStage.SetActive(false);
        panelQuests.SetActive(false);

        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowCampaign()
    {
        currentScene = SceneMenu.Campaign;

        panelMainMenu.SetActive(false);
        panelSoldier.SetActive(false);
        panelWeapon.SetActive(false);
        panelSelectStage.SetActive(true);
        panelQuests.SetActive(false);

        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowWeapon()
    {
        currentScene = SceneMenu.Weapons;

        panelMainMenu.SetActive(false);
        panelSoldier.SetActive(false);
        panelWeapon.SetActive(true);
        panelSelectStage.SetActive(false);
        panelQuests.SetActive(false);

        SoundManager.Instance.PlaySfxClick();

        //FirebaseAnalyticsHelper.LogEvent("EnterUpgradeWeapon");
    }

    public void ShowQuests()
    {
        currentScene = SceneMenu.Quests;

        panelMainMenu.SetActive(false);
        panelSoldier.SetActive(false);
        panelWeapon.SetActive(false);
        panelSelectStage.SetActive(false);
        panelQuests.SetActive(true);

        SoundManager.Instance.PlaySfxClick();

        if (GameData.isShowingTutorial)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepClickMission);
        }
    }

    public void ShowDailyGift()
    {
        //currentScene = SceneMenu.DailyGift;
        panelDailyGift.SetActive(true);
        isWaitingShowDailyGift = false;

        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_SHOW_DAILY_GIFT);
    }

    public void HideDailyGift()
    {
        //currentScene = SceneMenu.Main;
        //panelMainMenu.SetActive(true);
        panelDailyGift.SetActive(false);

        if (ProfileManager.UserProfile.isReceivedDailyGiftToday == false)
        {
            btnShowDailyGift.enabled = true;
        }
    }

    public void ShowFreeGift()
    {
        freeGiftController.Open();

        SoundManager.Instance.PlaySfxClick();

        if (GameData.isShowingTutorial && currentScene == SceneMenu.Main)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepClickButtonFreeGift);
        }
    }

    public void ShowMainMenu()
    {
        panelMainMenu.SetActive(true);
        currentScene = SceneMenu.Main;

        if (isWaitingShowDailyGift)
        {
            ShowDailyGift();
        }
        else if (isNewVersionAvailable)
        {
            ShowNewVersionUpdate();
        }

        CheckTutorial();
        CheckAllNotifications();
    }

    private void CheckTutorial()
    {
        if (GameData.playerTutorials.IsCompletedStep(TutorialType.WorldMap))
        {
            if (GameData.playerTutorials.IsCompletedStep(TutorialType.Mission) == false)
            {
                TutorialMenuController.Instance.ShowTutorial(TutorialType.Mission);
            }
            else if (GameData.playerTutorials.IsCompletedStep(TutorialType.FreeGift) == false)
            {
                TutorialMenuController.Instance.ShowTutorial(TutorialType.FreeGift);
            }
        }
    }

    #endregion


    #region TOURNAMENT

    public void StartChallenge(int priceEntrance)
    {
        Popup.Instance.ShowInstantLoading();

        // Verify game version + time after start challenge
        MasterInfo.Instance.StartGetData(true, res =>
        {
            Popup.Instance.HideInstantLoading();

            if (res != null)
            {
                if (res.code == (int)AppStatus.ForceUpdate)
                {
                    Popup.Instance.Show(
                        "Please update to the latest version to join Tournament",
                        "Update",
                        PopupType.Ok,
                        () =>
                        {
                            Utility.OpenStore();
                            isNewVersionAvailable = false;
                        });
                }
                else
                {
                    // Start Challenge Here
                    FireBaseDatabase.Instance.timeStartChallenge = MasterInfo.Instance.GetCurrentDateTime();

                    int countPlay = ProfileManager.UserProfile.countPlayTournament;
                    countPlay++;
                    ProfileManager.UserProfile.countPlayTournament.Set(countPlay);
                    GameData.playerResources.ConsumeGem(priceEntrance);
                    if (priceEntrance <= 0)
                        GameData.playerResources.ConsumeTournamentTicket(1);
                    GameData.mode = GameMode.Survival;
                    Loading.nextScene = StaticValue.SCENE_GAME_PLAY;
                    Popup.Instance.loading.Show();

                    FirebaseAnalyticsHelper.LogEvent("N_StartTournament", "Times=" + countPlay);
                }
            }
            else
            {
                Popup.Instance.Show("Data verification failed. Please try again!");
            }
        });
    }

    public void PlaySurvival()
    {
        GameData.mode = GameMode.Survival;
        Loading.nextScene = StaticValue.SCENE_GAME_PLAY;
        Popup.Instance.loading.Show();
    }

    public void LoginFacebookToJoinTournament()
    {
        popupLoginFacebook.SetActive(false);
        Popup.Instance.ShowInstantLoading();
        FbController.Instance.LoginWithReadPermission(LoginFacebookCallback);
    }

    private void FillTournamentData(List<TournamentData> data)
    {
        currentScene = SceneMenu.Tournament;

        FireBaseDatabase.Instance.GetTournamentScore(AccessToken.CurrentAccessToken.UserId, MasterInfo.Instance.GetCurrentWeekRangeString(), score =>
        {
            Popup.Instance.HideInstantLoading();

            if (GameData.playerTournamentData.score < score)
                GameData.playerTournamentData.score = score;

            hudTournament.Open(data);
        });

        FirebaseAnalyticsHelper.LogEvent("N_EnterMenuTournament");
    }

    private bool IsAvailablePlayTournament()
    {
        bool passed = MapUtils.IsStagePassed("1.3", Difficulty.Normal);

        return passed;
    }

    #endregion


    #region NOTIFICATION

    private void OnCheckTimeNewDayDone()
    {
        btnShowDailyGift.enabled = true;
        CheckAllNotifications();
    }

    private void CheckAllNotifications()
    {
        CheckNotificationWeapon();
        CheckNotificationSoldier();
        CheckNotificationQuests();
        CheckNotificationDailyGift();
        CheckNotificationFreeGifts();
        CheckNotificationTournament();
    }

    private void CheckNotificationWeapon()
    {
        bool hasNewWeapon = false;

        foreach (KeyValuePair<int, PlayerGunData> gun in GameData.playerGuns)
        {
            if (gun.Value.isNew)
            {
                hasNewWeapon = true;
                break;
            }
        }

        notiWeapon.SetActive(hasNewWeapon);
    }

    private void CheckNotificationSoldier()
    {
        int availablePoints = 0;

        foreach (KeyValuePair<int, PlayerRamboSkillData> pair in GameData.playerRamboSkills)
        {
            int points = GameData.playerRamboSkills.GetUnusedSkillPoints(pair.Key);

            availablePoints += points;
        }

        if (availablePoints > 0)
        {
            numberUnusedSkillPoints.text = availablePoints.ToString();
            numberUnusedSkillPoints.transform.parent.gameObject.SetActive(true);
            notiSoldier.SetActive(true);
        }
        else
        {
            numberUnusedSkillPoints.transform.parent.gameObject.SetActive(false);
            notiSoldier.SetActive(false);
        }

    }

    private void CheckNotificationQuests()
    {
        int readyDailyQuest = GameData.playerDailyQuests.GetNumberReadyQuest();
        int readyAchievement = GameData.playerAchievements.GetNumberReadyAchievement();
        int total = readyDailyQuest + readyAchievement;

        notiQuests.text = total.ToString();
        notiQuests.transform.parent.gameObject.SetActive(total > 0);
    }

    private void CheckNotificationDailyGift()
    {
        notiDailyGift.SetActive(!ProfileManager.UserProfile.isReceivedDailyGiftToday);
    }

    private void CheckNotificationFreeGifts()
    {
        freeGiftController.CheckNotification();
    }

    private void CheckNotificationTournament()
    {
        if (AccessToken.CurrentAccessToken != null)
        {
            //bool hasFreeEntrance = ProfileManager.UserProfile.countPlayTournament < StaticValue.TOURNAMENT_FREE_ENTRANCE;
            bool canPlay = GameData.playerResources.tournamentTicket > 0 && ProfileManager.UserProfile.countPlayTournament < StaticValue.TOURNAMENT_MAX_ENTRANCE;
            int numberUnclaimRewards = hudTournament.GetUnclaimRankRewards();

            notiTournament.SetActive(canPlay || numberUnclaimRewards > 0);
        }
        else
        {
            notiTournament.SetActive(false);
        }
    }

    private void ShowNewVersionUpdate()
    {
        Popup.Instance.Show(
                "New version available, please update the game",
                "Update",
                PopupType.Ok,
                () =>
                {
                    Utility.OpenStore();
                    isNewVersionAvailable = false;
                });
    }

    private void ShowRecommends()
    {
        if (GameData.isShowStarterPack == false)
        {
            if (MapUtils.IsStagePassed("1.1", Difficulty.Normal))
            {
                GameData.isShowStarterPack = true;

                if (ProfileManager.UserProfile.isPurchasedStarterPack == false)
                {
                    ShowStarterPack(true);
                }
            }
        }
        else if (GameData.isShowSpecialOffer == false)
        {
            if (MapUtils.IsStagePassed("1.1", Difficulty.Normal))
            {
                GameData.isShowSpecialOffer = true;

                if (specialOfferController.isShowPack)
                {
                    specialOfferController.Show(true);
                }
            }
        }
        //else if (ProfileManager.UserProfile.isShowUnlockTournament == false)
        //{
        //    ProfileManager.UserProfile.isShowUnlockTournament.Set(true);

        //    if (IsAvailablePlayTournament())
        //    {
        //        Popup.Instance.Show(
        //            content: "you have now unlocked <color=yellow>tournament mode</color> !\n Do you want to try?",
        //            title: "tournament",
        //            type: PopupType.YesNo,
        //            yesCallback: ShowTournament);
        //    }
        //}
    }

    #endregion


    #region CALLBACK

    private void LoginFacebookCallback(bool success)
    {
        if (success)
        {
            FbController.Instance.GetLoggedInUserInfomation(info =>
            {
                FireBaseDatabase.Instance.AuthenWithFacebook(AccessToken.CurrentAccessToken.UserId, AccessToken.CurrentAccessToken.TokenString, AuthenFirebaseCallback);
            });
        }
        else
        {
            Popup.Instance.HideInstantLoading();
            Popup.Instance.ShowToastMessage("Failed to login to Facebook!");
        }
    }

    private void AuthenFirebaseCallback(UserInfo authUserInfo)
    {
        if (authUserInfo != null)
        {
            GameData.playerTournamentData.id = authUserInfo.id;
            GameData.playerTournamentData.fbName = authUserInfo.name;

            if (GameData.playerTournamentData.sprAvatar == null)
            {
                FbController.Instance.GetProfilePictureById(authUserInfo.id, sprite =>
                {
                    GameData.playerTournamentData.sprAvatar = sprite;
                    hudTournament.SetPlayerFbAvatar(sprite);
                });
            }

            FireBaseDatabase.Instance.GetTopTournament(50, MasterInfo.Instance.GetCurrentWeekRangeString(), data =>
            {
                FillTournamentData(data);
            });
        }
        else
        {
            Popup.Instance.HideInstantLoading();
            Popup.Instance.Show("Authentication failed!");
            //Popup.Instance.ShowToastMessage("Authentication failed!", ToastLength.Long);
        }
    }

    #endregion
}
