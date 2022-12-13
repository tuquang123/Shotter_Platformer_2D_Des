using G2.Sdk.PlayerPrefsHelper;
using G2.Sdk.SecurityHelper;
using Newtonsoft.Json;

public class UserProfile
{
    public IntProfileData ramboId;

    public IntProfileData gunNormalId;
    public IntProfileData gunSpecialId;
    public IntProfileData grenadeId;
    public IntProfileData meleeWeaponId;

    public StringProfileData playerProfile;
    public StringProfileData playerRamboData;
    public StringProfileData playerGunData;
    public StringProfileData playerGrenadeData;
    public StringProfileData playerMeleeWeaponData;
    public StringProfileData playerCampaignProgessData; // Old
    public StringProfileData playerCampaignStageProgessData; // New
    public StringProfileData playerCampaignRewardProgessData;
    public StringProfileData playerResourcesData;
    public StringProfileData playerBoosterData;
    public StringProfileData playerSelectingBooster;
    public StringProfileData playerDailyQuestData;
    public StringProfileData playerAchievementData;
    public StringProfileData playerRamboSkillData;
    public StringProfileData playerTutorialData;

    public DateTimeProfileData dateLastLogin;

    public DateTimeProfileData dateGetGift;
    public IntProfileData getDailyGiftDay;
    public BoolProfileData isReceivedDailyGiftToday;
    public BoolProfileData isPassFirstWeek;

    public IntProfileData countViewAdsFreeCoin;
    public IntProfileData countShareFacebook;
    public IntProfileData countRewardInterstitialAds;

    public FloatProfileData soundVolume;
    public FloatProfileData musicVolume;

    public BoolProfileData isRemoveAds;

    public StringProfileData weekLastLogin;
    public IntProfileData countPlayTournament;
    public BoolProfileData isClaimedRank1;
    public BoolProfileData isClaimedRank2;
    public BoolProfileData isClaimedRank3;
    public BoolProfileData isClaimedRank4;
    public BoolProfileData isClaimedRank5;
    public BoolProfileData isClaimedRank6;
    public BoolProfileData isShowUnlockTournament;
    public StringProfileData tournamentGunProfile;

    public BoolProfileData isNoLongerRate;
    public BoolProfileData isShowRateMap1;
    public BoolProfileData isShowRateMap2;
    public BoolProfileData isShowRateMap3;

    public BoolProfileData isFirstBuyGem100;
    public BoolProfileData isFirstBuyGem300;
    public BoolProfileData isFirstBuyGem500;
    public BoolProfileData isFirstBuyGem1000;
    public BoolProfileData isFirstBuyGem2500;
    public BoolProfileData isFirstBuyGem5000;

    public BoolProfileData isPurchasedStarterPack;
    public BoolProfileData isPurchasedPackEverybodyFavorite;
    public BoolProfileData isPurchasedPackDragonBreath;
    public BoolProfileData isPurchasedPackLetThereBeFire;
    public BoolProfileData isPurchasedPackSnippingForDummies;
    public BoolProfileData isPurchasedPackTaserLaser;
    public BoolProfileData isPurchasedPackShockingSale;
    public BoolProfileData isPurchasedPackUpgradeEnthusiast;

    public UserProfile()
    {

    }

    public UserProfile(DataEncryption dataEncryption)
    {
        ramboId = new IntProfileData("current_rambo_id", StaticValue.RAMBO_ID_JOHN, dataEncryption);

        gunSpecialId = new IntProfileData("gun_special_id", -1, dataEncryption);
        gunNormalId = new IntProfileData("gun_normal_id", StaticValue.GUN_ID_UZI, dataEncryption);
        grenadeId = new IntProfileData("grenade_id", StaticValue.GRENADE_ID_F1, dataEncryption);
        meleeWeaponId = new IntProfileData("melee_weapon_id", StaticValue.MELEE_WEAPON_ID_KNIFE, dataEncryption);

        playerProfile = new StringProfileData("player_profile", string.Empty, dataEncryption);
        playerRamboData = new StringProfileData("player_rambo_data", string.Empty, dataEncryption);
        playerGunData = new StringProfileData("player_gun_data", string.Empty, dataEncryption);
        playerGrenadeData = new StringProfileData("player_grenades_data", string.Empty, dataEncryption);
        playerMeleeWeaponData = new StringProfileData("player_melee_weapon_data", string.Empty, dataEncryption);
        playerCampaignProgessData = new StringProfileData("player_campaign_progress_data", string.Empty, dataEncryption);
        playerCampaignStageProgessData = new StringProfileData("player_campaign_stage_progress_data", string.Empty, dataEncryption);
        playerCampaignRewardProgessData = new StringProfileData("player_campaign_reward_progress_data", string.Empty, dataEncryption);
        playerResourcesData = new StringProfileData("player_resources_data", string.Empty, dataEncryption);
        playerBoosterData = new StringProfileData("player_booster_data", string.Empty, dataEncryption);
        playerSelectingBooster = new StringProfileData("player_selecting_booster", string.Empty, dataEncryption);
        playerDailyQuestData = new StringProfileData("player_daily_quest_data", string.Empty, dataEncryption);
        playerAchievementData = new StringProfileData("player_achievement_data", string.Empty, dataEncryption);
        playerRamboSkillData = new StringProfileData("player_rambo_skill_data", string.Empty, dataEncryption);
        playerTutorialData = new StringProfileData("player_tutorial_data", string.Empty, dataEncryption);

        dateLastLogin = new DateTimeProfileData("data_last_login", StaticValue.defaultDate, dataEncryption);

        dateGetGift = new DateTimeProfileData("date_get_gift", StaticValue.defaultDate, dataEncryption);
        getDailyGiftDay = new IntProfileData("get_daily_gift_day", 1, dataEncryption);
        isReceivedDailyGiftToday = new BoolProfileData("is_received_daily_gift_today", false, dataEncryption);
        isPassFirstWeek = new BoolProfileData("is_pass_first_week", false, dataEncryption);

        countViewAdsFreeCoin = new IntProfileData("count_view_ads_free_coin", 0, dataEncryption);
        countShareFacebook = new IntProfileData("count_share_facebook", 0, dataEncryption);
        countRewardInterstitialAds = new IntProfileData("count_reward_interstitial_ads", 0, dataEncryption);

        soundVolume = new FloatProfileData("sound_volume", 1f, dataEncryption);
        musicVolume = new FloatProfileData("music_volume", 1f, dataEncryption);

        isRemoveAds = new BoolProfileData("remove_ads", false, dataEncryption);

        weekLastLogin = new StringProfileData("week_last_login", string.Empty, dataEncryption);
        countPlayTournament = new IntProfileData("count_play_tournament", 0, dataEncryption);
        isClaimedRank1 = new BoolProfileData("is_claimed_rank_1", false, dataEncryption);
        isClaimedRank2 = new BoolProfileData("is_claimed_rank_2", false, dataEncryption);
        isClaimedRank3 = new BoolProfileData("is_claimed_rank_3", false, dataEncryption);
        isClaimedRank4 = new BoolProfileData("is_claimed_rank_4", false, dataEncryption);
        isClaimedRank5 = new BoolProfileData("is_claimed_rank_5", false, dataEncryption);
        isClaimedRank6 = new BoolProfileData("is_claimed_rank_6", false, dataEncryption);
        isShowUnlockTournament = new BoolProfileData("is_show_unlock_tournament", false, dataEncryption);
        tournamentGunProfile = new StringProfileData("tournament_gun_profile", string.Empty, dataEncryption);

        isNoLongerRate = new BoolProfileData("is_no_longer_rate", false, dataEncryption);
        isShowRateMap1 = new BoolProfileData("is_show_rate_map_1", false, dataEncryption);
        isShowRateMap2 = new BoolProfileData("is_show_rate_map_2", false, dataEncryption);
        isShowRateMap3 = new BoolProfileData("is_show_rate_map_3", false, dataEncryption);

        isFirstBuyGem100 = new BoolProfileData("is_first_buy_gem_100", false, dataEncryption);
        isFirstBuyGem300 = new BoolProfileData("is_first_buy_gem_300", false, dataEncryption);
        isFirstBuyGem500 = new BoolProfileData("is_first_buy_gem_500", false, dataEncryption);
        isFirstBuyGem1000 = new BoolProfileData("is_first_buy_gem_1000", false, dataEncryption);
        isFirstBuyGem2500 = new BoolProfileData("is_first_buy_gem_2500", false, dataEncryption);
        isFirstBuyGem5000 = new BoolProfileData("is_first_buy_gem_5000", false, dataEncryption);

        isPurchasedStarterPack = new BoolProfileData("is_purchased_starter_pack", false, dataEncryption);
        isPurchasedPackEverybodyFavorite = new BoolProfileData("is_purchased_pack_everybody_favorite", false, dataEncryption);
        isPurchasedPackDragonBreath = new BoolProfileData("is_purchased_pack_dragon_breath", false, dataEncryption);
        isPurchasedPackLetThereBeFire = new BoolProfileData("is_purchased_pack_let_there_be_fire", false, dataEncryption);
        isPurchasedPackSnippingForDummies = new BoolProfileData("is_purchased_pack_snipping_for_dummies", false, dataEncryption);
        isPurchasedPackTaserLaser = new BoolProfileData("is_purchased_pack_taser_laser", false, dataEncryption);
        isPurchasedPackShockingSale = new BoolProfileData("is_purchased_pack_shocking_sale", false, dataEncryption);
        isPurchasedPackUpgradeEnthusiast = new BoolProfileData("is_purchased_pack_upgrade_enthusiast", false, dataEncryption);
    }

    public void ResetTo(RawUserProfile newData)
    {
        if (newData == null)
            return;

        ramboId.Set(newData.ramboId);

        gunSpecialId.Set(newData.gunSpecialId);
        gunNormalId.Set(newData.gunNormalId);
        grenadeId.Set(newData.grenadeId);
        meleeWeaponId.Set(newData.meleeWeaponId);

        playerProfile.Set(newData.playerProfile);
        playerRamboData.Set(newData.playerRamboData);
        playerGunData.Set(newData.playerGunData);
        playerGrenadeData.Set(newData.playerGrenadeData);
        playerMeleeWeaponData.Set(newData.playerMeleeWeaponData);
        playerCampaignProgessData.Set(newData.playerCampaignProgessData);
        playerCampaignStageProgessData.Set(newData.playerCampaignStageProgessData);
        playerCampaignRewardProgessData.Set(newData.playerCampaignRewardProgessData);
        playerResourcesData.Set(newData.playerResourcesData);
        playerBoosterData.Set(newData.playerBoosterData);
        playerSelectingBooster.Set(newData.playerSelectingBooster);
        playerDailyQuestData.Set(newData.playerDailyQuestData);
        playerAchievementData.Set(newData.playerAchievementData);
        playerRamboSkillData.Set(newData.playerRamboSkillData);
        playerTutorialData.Set(newData.playerTutorialData);

        dateLastLogin.Set(newData.dateLastLogin);

        dateGetGift.Set(newData.dateGetGift);
        getDailyGiftDay.Set(newData.getDailyGiftDay);
        isReceivedDailyGiftToday.Set(newData.isReceivedDailyGiftToday);
        isPassFirstWeek.Set(newData.isPassFirstWeek);

        countViewAdsFreeCoin.Set(newData.countViewAdsFreeCoin);
        countShareFacebook.Set(newData.countShareFacebook);
        countRewardInterstitialAds.Set(newData.countRewardInterstitialAds);

        soundVolume.Set(newData.soundVolume);
        musicVolume.Set(newData.musicVolume);

        isRemoveAds.Set(newData.isRemoveAds);

        weekLastLogin.Set(newData.weekLastLogin);
        countPlayTournament.Set(newData.countPlayTournament);
        isClaimedRank1.Set(newData.isClaimedRank1);
        isClaimedRank2.Set(newData.isClaimedRank2);
        isClaimedRank3.Set(newData.isClaimedRank3);
        isClaimedRank4.Set(newData.isClaimedRank4);
        isClaimedRank5.Set(newData.isClaimedRank5);
        isClaimedRank6.Set(newData.isClaimedRank6);
        isShowUnlockTournament.Set(newData.isShowUnlockTournament);
        tournamentGunProfile.Set(newData.tournamentGunProfile);

        isNoLongerRate.Set(newData.isNoLongerRate);
        isShowRateMap1.Set(newData.isShowRateMap1);
        isShowRateMap2.Set(newData.isShowRateMap2);
        isShowRateMap3.Set(newData.isShowRateMap3);

        isFirstBuyGem100.Set(newData.isFirstBuyGem100);
        isFirstBuyGem300.Set(newData.isFirstBuyGem300);
        isFirstBuyGem500.Set(newData.isFirstBuyGem500);
        isFirstBuyGem1000.Set(newData.isFirstBuyGem1000);
        isFirstBuyGem2500.Set(newData.isFirstBuyGem2500);
        isFirstBuyGem5000.Set(newData.isFirstBuyGem5000);

        isPurchasedStarterPack.Set(newData.isPurchasedStarterPack);
        isPurchasedPackEverybodyFavorite.Set(newData.isPurchasedPackEverybodyFavorite);
        isPurchasedPackDragonBreath.Set(newData.isPurchasedPackDragonBreath);
        isPurchasedPackLetThereBeFire.Set(newData.isPurchasedPackLetThereBeFire);
        isPurchasedPackSnippingForDummies.Set(newData.isPurchasedPackSnippingForDummies);
        isPurchasedPackTaserLaser.Set(newData.isPurchasedPackTaserLaser);
        isPurchasedPackShockingSale.Set(newData.isPurchasedPackShockingSale);
        isPurchasedPackUpgradeEnthusiast.Set(newData.isPurchasedPackUpgradeEnthusiast);

        DebugCustom.Log("**** RESTORE DATA COMPLETE ****");
    }

    public RawUserProfile GetRawUserProfile()
    {
        RawUserProfile raw = new RawUserProfile();

        raw.ramboId = ramboId.data;

        raw.gunSpecialId = gunSpecialId.data;
        raw.gunNormalId = gunNormalId.data;
        raw.grenadeId = grenadeId.data;
        raw.meleeWeaponId = meleeWeaponId.data;

        raw.playerProfile = playerProfile.data;
        raw.playerRamboData = playerRamboData.data;
        raw.playerGunData = playerGunData.data;
        raw.playerGrenadeData = playerGrenadeData.data;
        raw.playerMeleeWeaponData = playerMeleeWeaponData.data;
        raw.playerCampaignProgessData = playerCampaignProgessData.data;
        raw.playerCampaignStageProgessData = playerCampaignStageProgessData.data;
        raw.playerCampaignRewardProgessData = playerCampaignRewardProgessData.data;
        raw.playerResourcesData = playerResourcesData.data;
        raw.playerBoosterData = playerBoosterData.data;
        raw.playerSelectingBooster = playerSelectingBooster.data;
        raw.playerDailyQuestData = playerDailyQuestData.data;
        raw.playerAchievementData = playerAchievementData.data;
        raw.playerRamboSkillData = playerRamboSkillData.data;
        raw.playerTutorialData = playerTutorialData;

        raw.dateLastLogin = dateLastLogin.data;

        raw.dateGetGift = dateGetGift.data;
        raw.getDailyGiftDay = getDailyGiftDay.data;
        raw.isReceivedDailyGiftToday = isReceivedDailyGiftToday.data;
        raw.isPassFirstWeek = isPassFirstWeek.data;

        raw.countViewAdsFreeCoin = countViewAdsFreeCoin.data;
        raw.countShareFacebook = countShareFacebook.data;
        raw.countRewardInterstitialAds = countRewardInterstitialAds;

        raw.soundVolume = soundVolume.data;
        raw.musicVolume = musicVolume.data;

        raw.isRemoveAds = isRemoveAds.data;

        raw.weekLastLogin = weekLastLogin.data;
        raw.countPlayTournament = countPlayTournament.data;
        raw.isClaimedRank1 = isClaimedRank1.data;
        raw.isClaimedRank2 = isClaimedRank2.data;
        raw.isClaimedRank3 = isClaimedRank3.data;
        raw.isClaimedRank4 = isClaimedRank4.data;
        raw.isClaimedRank5 = isClaimedRank5.data;
        raw.isClaimedRank6 = isClaimedRank6.data;
        raw.isShowUnlockTournament = isShowUnlockTournament;
        raw.tournamentGunProfile = tournamentGunProfile.data;

        raw.isNoLongerRate = isNoLongerRate;
        raw.isShowRateMap1 = isShowRateMap1;
        raw.isShowRateMap2 = isShowRateMap2;
        raw.isShowRateMap3 = isShowRateMap3;

        raw.isFirstBuyGem100 = isFirstBuyGem100;
        raw.isFirstBuyGem300 = isFirstBuyGem300;
        raw.isFirstBuyGem500 = isFirstBuyGem500;
        raw.isFirstBuyGem1000 = isFirstBuyGem1000;
        raw.isFirstBuyGem2500 = isFirstBuyGem2500;
        raw.isFirstBuyGem5000 = isFirstBuyGem5000;

        raw.isPurchasedStarterPack = isPurchasedStarterPack;
        raw.isPurchasedPackEverybodyFavorite = isPurchasedPackEverybodyFavorite;
        raw.isPurchasedPackDragonBreath = isPurchasedPackDragonBreath;
        raw.isPurchasedPackLetThereBeFire = isPurchasedPackLetThereBeFire;
        raw.isPurchasedPackSnippingForDummies = isPurchasedPackSnippingForDummies;
        raw.isPurchasedPackTaserLaser = isPurchasedPackTaserLaser;
        raw.isPurchasedPackShockingSale = isPurchasedPackShockingSale;
        raw.isPurchasedPackUpgradeEnthusiast = isPurchasedPackUpgradeEnthusiast;

        return raw;
    }

    public string GetRawUserProfileJsonString()
    {
        return JsonConvert.SerializeObject(GetRawUserProfile());
    }
}