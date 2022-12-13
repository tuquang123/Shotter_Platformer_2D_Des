using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameData
{
    public static GameMode mode = GameMode.Campaign;

    // World map
    //public static Difficulty selectingDifficulty = Difficulty.Normal;
    public static StageData currentStage;

    // Game play
    public static bool isAutoFire;
    public static bool isAutoCollectCoin;
    public static bool isUseRevive;

    // Main Menu
    public static bool isShowStarterPack;
    public static bool isShowSpecialOffer;
    public static bool isShowingTutorial;

    // Tournament
    public static PlayerTournamentData playerTournamentData = new PlayerTournamentData();
    public static BoosterType survivalUsingBooster = BoosterType.None;

    // Mix
    public static Dictionary<string, string> questDescriptions;
    public static Dictionary<int, string> rankNames;
    public static Dictionary<int, int> gunValueGem;
    public static Dictionary<string, int> campaignStageLevelData;

    // Static data
    public static _StaticRecommendGunData staticRecommendGunData;
    public static _StaticRamboData staticRamboData;
    public static _StaticRamboSkillData staticRamboSkillData;
    public static _StaticGunData staticGunData;
    public static _StaticGrenadeData staticGrenadeData;
    public static _StaticMeleeWeaponData staticMeleeWeaponData;
    public static _StaticCampaignStageData staticCampaignStageData;
    public static _StaticCampaignBoxRewardData staticCampaignBoxRewardData;
    public static _StaticBoosterData staticBoosterData;
    public static _StaticDailyQuestData staticDailyQuestData;
    public static _StaticAchievementData staticAchievementData;
    public static _StaticRankData staticRankData;
    public static _StaticFreeGiftData staticFreeGiftData;
    public static _StaticTournamentRankData staticTournamentRankData;
    public static Dictionary<int, List<RewardData>> tournamentTopRankRewards;

    // Player data
    public static _PlayerProfile playerProfile;
    public static _PlayerResourcesData playerResources;
    public static _PlayerRamboData playerRambos;
    public static _PlayerRamboSkillData playerRamboSkills;
    public static _PlayerGunData playerGuns;
    public static _PlayerGrenadeData playerGrenades;
    public static _PlayerMeleeWeaponData playerMeleeWeapons;
    public static _PlayerCampaignProgressData playerCampaignProgress; // Old
    public static _PlayerCampaignStageProgressData playerCampaignStageProgress; // New
    public static _PlayerCampaignRewardProgressData playerCampaignRewardProgress;
    public static _PlayerBoosterData playerBoosters;
    public static _PlayerSelectingBooster selectingBoosters;
    public static _PlayerDailyQuestData playerDailyQuests;
    public static _PlayerAchievementData playerAchievements;
    public static _PlayerTutorialData playerTutorials;

    // Free Gift
    public static int durationNextGift;
    public static float timeCloseFreeGift;
}
