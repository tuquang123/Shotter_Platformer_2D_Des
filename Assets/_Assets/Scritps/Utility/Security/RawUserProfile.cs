using Newtonsoft.Json;
using System;

public class RawUserProfile
{
    public int ramboId;

    public int gunNormalId;
    public int gunSpecialId;
    public int grenadeId;
    public int meleeWeaponId;

    public string playerProfile;
    public string playerRamboData;
    public string playerGunData;
    public string playerGrenadeData;
    public string playerMeleeWeaponData;
    public string playerCampaignProgessData; // Old
    public string playerCampaignStageProgessData; // New
    public string playerCampaignRewardProgessData;
    public string playerResourcesData;
    public string playerBoosterData;
    public string playerSelectingBooster;
    public string playerDailyQuestData;
    public string playerAchievementData;
    public string playerRamboSkillData;
    public string playerTutorialData;

    public DateTime dateLastLogin;

    public DateTime dateGetGift;
    public int getDailyGiftDay;
    public bool isReceivedDailyGiftToday;
    public bool isPassFirstWeek;

    public int countViewAdsFreeCoin;
    public int countShareFacebook;
    public int countRewardInterstitialAds;

    [JsonIgnore]
    public float soundVolume = 1;
    [JsonIgnore]
    public float musicVolume = 1;

    public bool isRemoveAds;

    public string weekLastLogin;
    public int countPlayTournament;
    public bool isClaimedRank1;
    public bool isClaimedRank2;
    public bool isClaimedRank3;
    public bool isClaimedRank4;
    public bool isClaimedRank5;
    public bool isClaimedRank6;
    public bool isShowUnlockTournament;
    public string tournamentGunProfile;

    [JsonIgnore]
    public bool isNoLongerRate;
    [JsonIgnore]
    public bool isShowRateMap1;
    [JsonIgnore]
    public bool isShowRateMap2;
    [JsonIgnore]
    public bool isShowRateMap3;

    public bool isFirstBuyGem100;
    public bool isFirstBuyGem300;
    public bool isFirstBuyGem500;
    public bool isFirstBuyGem1000;
    public bool isFirstBuyGem2500;
    public bool isFirstBuyGem5000;

    public bool isPurchasedStarterPack;
    public bool isPurchasedPackEverybodyFavorite;
    public bool isPurchasedPackDragonBreath;
    public bool isPurchasedPackLetThereBeFire;
    public bool isPurchasedPackSnippingForDummies;
    public bool isPurchasedPackTaserLaser;
    public bool isPurchasedPackShockingSale;
    public bool isPurchasedPackUpgradeEnthusiast;
}
