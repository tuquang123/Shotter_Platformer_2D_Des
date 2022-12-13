using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class Login : MonoBehaviour
{
    public Text textVersion;

    private void Awake()
    {
        LoadStaticData();
        LoadPlayerData();
        //LoadIapRewardData();
    }

    private void Start()
    {
        textVersion.text = "v" + MasterInfo.Instance.Version;
    }

    public void PlayAsGuest()
    {
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
    }


    #region LOAD JSON DATA

    private void LoadStaticData()
    {
        LoadStaticGunData();
        LoadStaticGrenadeData();
        LoadStaticMeleeWeaponData();
        LoadStaticCampaignStageData();
        LoadStaticCampaignBoxRewardData();
        LoadStaticRecommendGunData();
        LoadStaticRamboData();
        LoadStaticRamboSkillData();
        LoadStaticBoosterData();
        LoadStaticDailyQuestData();
        LoadStaticAchievementData();
        LoadStaticRankData();
        LoadStaticFreeGiftData();
        LoadStaticTournamentRankData();

        LoadQuestDescription();
        LoadRankName();
        LoadGunValueGem();
        LoadCampaignStageLevelData();
    }

    private void LoadStaticGunData()
    {
        if (GameData.staticGunData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_GUN_DATA);
            //GameData.staticGunData = JsonConvert.DeserializeObject<_StaticGunData>(textAsset.text);

            Dictionary<int, StaticGunData> dict = JsonConvert.DeserializeObject<Dictionary<int, StaticGunData>>(textAsset.text);
            List<KeyValuePair<int, StaticGunData>> myList = dict.ToList();
            myList = myList.OrderBy(x => x.Value.isSpecialGun).ThenBy(x => x.Value.index).ToList();
            dict = myList.ToDictionary(x => x.Key, x => x.Value);
            string s = JsonConvert.SerializeObject(dict);
            GameData.staticGunData = JsonConvert.DeserializeObject<_StaticGunData>(s);
        }

        DebugCustom.Log("StaticGunData=" + JsonConvert.SerializeObject(GameData.staticGunData));
    }

    private void LoadStaticGrenadeData()
    {
        if (GameData.staticGrenadeData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_GRENADE_DATA);
            GameData.staticGrenadeData = JsonConvert.DeserializeObject<_StaticGrenadeData>(textAsset.text);
        }

        DebugCustom.Log("StaticGrenadeData=" + JsonConvert.SerializeObject(GameData.staticGrenadeData));
    }

    private void LoadStaticMeleeWeaponData()
    {
        if (GameData.staticMeleeWeaponData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_MELEE_WEAPON_DATA);
            GameData.staticMeleeWeaponData = JsonConvert.DeserializeObject<_StaticMeleeWeaponData>(textAsset.text);
        }

        DebugCustom.Log("StaticMeleeWeaponData=" + JsonConvert.SerializeObject(GameData.staticMeleeWeaponData));
    }

    private void LoadStaticCampaignStageData()
    {
        if (GameData.staticCampaignStageData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_CAMPAIGN_STAGE_DATA);
            GameData.staticCampaignStageData = JsonConvert.DeserializeObject<_StaticCampaignStageData>(textAsset.text);
        }

        DebugCustom.Log("StaticCampaignStageData=" + JsonConvert.SerializeObject(GameData.staticCampaignStageData));
    }

    private void LoadStaticCampaignBoxRewardData()
    {
        if (GameData.staticCampaignBoxRewardData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_CAMPAIGN_BOX_REWARD_DATA);
            GameData.staticCampaignBoxRewardData = JsonConvert.DeserializeObject<_StaticCampaignBoxRewardData>(textAsset.text);
        }

        DebugCustom.Log("StaticCampaignBoxRewardData=" + JsonConvert.SerializeObject(GameData.staticCampaignBoxRewardData));
    }

    private void LoadStaticRamboData()
    {
        if (GameData.staticRamboData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_RAMBO_DATA);
            GameData.staticRamboData = JsonConvert.DeserializeObject<_StaticRamboData>(textAsset.text);
        }

        DebugCustom.Log("StaticRamboData=" + JsonConvert.SerializeObject(GameData.staticRamboData));
    }

    private void LoadStaticRamboSkillData()
    {
        if (GameData.staticRamboSkillData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_RAMBO_SKILL_DATA);
            GameData.staticRamboSkillData = JsonConvert.DeserializeObject<_StaticRamboSkillData>(textAsset.text);
        }

        DebugCustom.Log("StaticRamboSkillData=" + JsonConvert.SerializeObject(GameData.staticRamboSkillData));
    }

    private void LoadStaticBoosterData()
    {
        if (GameData.staticBoosterData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_BOOSTER_DATA);
            GameData.staticBoosterData = JsonConvert.DeserializeObject<_StaticBoosterData>(textAsset.text);
        }

        DebugCustom.Log("StaticBoosterData=" + JsonConvert.SerializeObject(GameData.staticBoosterData));
    }

    private void LoadStaticDailyQuestData()
    {
        if (GameData.staticDailyQuestData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_DAILY_QUEST_DATA);
            GameData.staticDailyQuestData = JsonConvert.DeserializeObject<_StaticDailyQuestData>(textAsset.text);
        }

        DebugCustom.Log("StaticDailyQuestData=" + JsonConvert.SerializeObject(GameData.staticDailyQuestData));
    }

    private void LoadStaticAchievementData()
    {
        if (GameData.staticAchievementData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_ACHIEVEMENT_DATA);
            GameData.staticAchievementData = JsonConvert.DeserializeObject<_StaticAchievementData>(textAsset.text);
        }

        DebugCustom.Log("StaticAchievementData=" + JsonConvert.SerializeObject(GameData.staticAchievementData));
    }

    private void LoadStaticRankData()
    {
        if (GameData.staticRankData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_RANK_DATA);
            GameData.staticRankData = JsonConvert.DeserializeObject<_StaticRankData>(textAsset.text);
        }

        DebugCustom.Log("StaticRankData=" + JsonConvert.SerializeObject(GameData.staticRankData));
    }

    private void LoadStaticFreeGiftData()
    {
        if (GameData.staticFreeGiftData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_FREE_GIFT_DATA);
            GameData.staticFreeGiftData = JsonConvert.DeserializeObject<_StaticFreeGiftData>(textAsset.text);
        }

        DebugCustom.Log("StaticFreeGiftData=" + JsonConvert.SerializeObject(GameData.staticFreeGiftData));
    }

    private void LoadStaticTournamentRankData()
    {
        if (GameData.staticTournamentRankData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_TOURNAMENT_RANK_DATA);
            GameData.staticTournamentRankData = JsonConvert.DeserializeObject<_StaticTournamentRankData>(textAsset.text);
        }

        DebugCustom.Log("StaticTournamentRankData=" + JsonConvert.SerializeObject(GameData.staticTournamentRankData));

        if (GameData.tournamentTopRankRewards == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_TOURNAMENT_TOP_RANK_REWARD);
            GameData.tournamentTopRankRewards = JsonConvert.DeserializeObject<Dictionary<int, List<RewardData>>>(textAsset.text);
        }

        DebugCustom.Log("TournamentRankRewards=" + JsonConvert.SerializeObject(GameData.tournamentTopRankRewards));
    }

    private void LoadStaticRecommendGunData()
    {
        if (GameData.staticRecommendGunData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_RECOMMEND_GUN_DATA);
            GameData.staticRecommendGunData = JsonConvert.DeserializeObject<_StaticRecommendGunData>(textAsset.text);
            DebugCustom.Log("RecommendGunData=" + JsonConvert.SerializeObject(GameData.staticRecommendGunData));
        }
    }

    private void LoadQuestDescription()
    {
        if (GameData.questDescriptions == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STAGE_QUEST_DESCRIPTION);
            GameData.questDescriptions = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
        }
    }

    private void LoadRankName()
    {
        if (GameData.rankNames == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_RANK_NAME);
            GameData.rankNames = JsonConvert.DeserializeObject<Dictionary<int, string>>(textAsset.text);
        }
    }

    private void LoadGunValueGem()
    {
        if (GameData.gunValueGem == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_EXCHANGE_WEAPON_TO_GEM);
            GameData.gunValueGem = JsonConvert.DeserializeObject<Dictionary<int, int>>(textAsset.text);
        }
    }

    private void LoadCampaignStageLevelData()
    {
        if (GameData.campaignStageLevelData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_CAMPAIGN_STAGE_LEVEL_DATA);
            GameData.campaignStageLevelData = JsonConvert.DeserializeObject<Dictionary<string, int>>(textAsset.text);
        }
    }

    #endregion


    #region PLAYER DATA

    private void LoadPlayerData()
    {
        LoadPlayerProfile();
        LoadPlayerResourcesData();
        LoadPlayerRamboData();
        LoadPlayerRamboSkillData();
        LoadPlayerGunData();
        LoadPlayerGrenadeData();
        LoadPlayerMeleeWeaponData();
        LoadPlayerCampaignProgressData();
        LoadPlayerCampaignRewardProgressData();
        LoadPlayerBoosterData();
        LoadPlayerSelectingBooster();
        LoadPlayerDailyQuestData();
        LoadPlayerAchievementData();
        LoadPlayerTutorialData();

        ProfileManager.SaveAll();
    }

    private void LoadPlayerProfile()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerProfile))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_PROFILE).text;
            ProfileManager.UserProfile.playerProfile.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerProfile;
        }

        GameData.playerProfile = JsonConvert.DeserializeObject<_PlayerProfile>(s);
        DebugCustom.Log("PlayerProfile=" + JsonConvert.SerializeObject(GameData.playerProfile));
    }

    private void LoadPlayerResourcesData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerResourcesData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_RESOURCES_DATA).text;
            ProfileManager.UserProfile.playerResourcesData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerResourcesData;
        }

        GameData.playerResources = JsonConvert.DeserializeObject<_PlayerResourcesData>(s);
        DebugCustom.Log("PlayerResources=" + JsonConvert.SerializeObject(GameData.playerResources));
    }

    private void LoadPlayerRamboData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerRamboData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_RAMBO_DATA).text;
            ProfileManager.UserProfile.playerRamboData.Set(s);
            ProfileManager.UserProfile.ramboId.Set(StaticValue.RAMBO_ID_JOHN);
        }
        else
        {
            s = ProfileManager.UserProfile.playerRamboData;
        }

        GameData.playerRambos = JsonConvert.DeserializeObject<_PlayerRamboData>(s);
        DebugCustom.Log("PlayerRambos=" + JsonConvert.SerializeObject(GameData.playerRambos));
    }

    private void LoadPlayerRamboSkillData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerRamboSkillData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_RAMBO_SKILL_DATA).text;
            ProfileManager.UserProfile.playerRamboSkillData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerRamboSkillData;
        }

        GameData.playerRamboSkills = JsonConvert.DeserializeObject<_PlayerRamboSkillData>(s);
        DebugCustom.Log("PlayerRamboSkills=" + JsonConvert.SerializeObject(GameData.playerRamboSkills));
    }

    private void LoadPlayerGunData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerGunData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_GUN_DATA).text;
            ProfileManager.UserProfile.playerGunData.Set(s);
            ProfileManager.UserProfile.gunNormalId.Set(StaticValue.GUN_ID_UZI);
            ProfileManager.UserProfile.gunSpecialId.Set(-1);
        }
        else
        {
            s = ProfileManager.UserProfile.playerGunData;
        }

        GameData.playerGuns = JsonConvert.DeserializeObject<_PlayerGunData>(s);
        DebugCustom.Log("PlayerGuns=" + JsonConvert.SerializeObject(GameData.playerGuns));
    }

    private void LoadPlayerGrenadeData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerGrenadeData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_GRENADE_DATA).text;
            ProfileManager.UserProfile.playerGrenadeData.Set(s);
            ProfileManager.UserProfile.grenadeId.Set(StaticValue.GRENADE_ID_F1);
        }
        else
        {
            s = ProfileManager.UserProfile.playerGrenadeData;
        }

        GameData.playerGrenades = JsonConvert.DeserializeObject<_PlayerGrenadeData>(s);
        DebugCustom.Log("PlayerGrenades=" + JsonConvert.SerializeObject(GameData.playerGrenades));

        // Remove Dragon-Nades
        int grenadeId = ProfileManager.UserProfile.grenadeId;
        if (GameData.staticGrenadeData.ContainsKey(grenadeId) == false)
        {
            GameData.playerGrenades.RemoveGrenade(grenadeId);
            ProfileManager.UserProfile.grenadeId.Set(StaticValue.GRENADE_ID_F1);
        }
    }

    private void LoadPlayerMeleeWeaponData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerMeleeWeaponData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_MELEE_WEAPON_DATA).text;
            ProfileManager.UserProfile.playerMeleeWeaponData.Set(s);
            ProfileManager.UserProfile.meleeWeaponId.Set(StaticValue.MELEE_WEAPON_ID_KNIFE);
        }
        else
        {
            s = ProfileManager.UserProfile.playerMeleeWeaponData;
        }

        GameData.playerMeleeWeapons = JsonConvert.DeserializeObject<_PlayerMeleeWeaponData>(s);
        DebugCustom.Log("PlayerMeleeWeapons=" + JsonConvert.SerializeObject(GameData.playerMeleeWeapons));
    }

    private void LoadPlayerCampaignProgressData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerCampaignProgessData))
        {
            if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerCampaignStageProgessData))
            {
                s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_CAMPAIGN_STAGE_PROGRESS_DATA).text;
                ProfileManager.UserProfile.playerCampaignStageProgessData.Set(s);
            }
            else
            {
                s = ProfileManager.UserProfile.playerCampaignStageProgessData;
            }

            GameData.playerCampaignStageProgress = JsonConvert.DeserializeObject<_PlayerCampaignStageProgressData>(s);
        }
        else
        {
            GameData.playerCampaignProgress = JsonConvert.DeserializeObject<_PlayerCampaignProgressData>(ProfileManager.UserProfile.playerCampaignProgessData);
            DebugCustom.Log("PlayerCampaignProgress OLD=" + JsonConvert.SerializeObject(GameData.playerCampaignProgress));

            GameData.playerCampaignStageProgress = new _PlayerCampaignStageProgressData();

            foreach (KeyValuePair<Difficulty, PlayerCampaignProgressData> difficulty in GameData.playerCampaignProgress)
            {
                foreach (KeyValuePair<string, List<bool>> progress in difficulty.Value.stageProgress)
                {
                    if (GameData.playerCampaignStageProgress.ContainsKey(progress.Key))
                    {
                        GameData.playerCampaignStageProgress[progress.Key][(int)difficulty.Key] = true;
                    }
                    else
                    {
                        List<bool> listResult = new List<bool>();

                        for (int i = 0; i < 3; i++)
                        {
                            listResult.Add(i == (int)difficulty.Key);
                        }

                        GameData.playerCampaignStageProgress.Add(progress.Key, listResult);
                    }
                }
            }

            ProfileManager.UserProfile.playerCampaignProgessData.Set(string.Empty);
            ProfileManager.UserProfile.playerCampaignStageProgessData.Set(JsonConvert.SerializeObject(GameData.playerCampaignStageProgress));
        }

        DebugCustom.Log("PlayerCampaignProgress=" + JsonConvert.SerializeObject(GameData.playerCampaignStageProgress));
    }

    private void LoadPlayerCampaignRewardProgressData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerCampaignRewardProgessData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_CAMPAIGN_REWARD_PROGRESS_DATA).text;
            ProfileManager.UserProfile.playerCampaignRewardProgessData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerCampaignRewardProgessData;
        }

        GameData.playerCampaignRewardProgress = JsonConvert.DeserializeObject<_PlayerCampaignRewardProgressData>(s);
        DebugCustom.Log("PlayerCampaignRewardProgress=" + JsonConvert.SerializeObject(GameData.playerCampaignRewardProgress));
    }

    private void LoadPlayerBoosterData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerBoosterData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_BOOSTER_DATA).text;
            ProfileManager.UserProfile.playerBoosterData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerBoosterData;
        }

        GameData.playerBoosters = JsonConvert.DeserializeObject<_PlayerBoosterData>(s);
        DebugCustom.Log("PlayerBoosters=" + JsonConvert.SerializeObject(GameData.playerBoosters));
    }

    private void LoadPlayerSelectingBooster()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerSelectingBooster))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_SELECTING_BOOSTER).text;
            ProfileManager.UserProfile.playerSelectingBooster.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerSelectingBooster;
        }

        GameData.selectingBoosters = JsonConvert.DeserializeObject<_PlayerSelectingBooster>(s);
        DebugCustom.Log("PlayerSelectingBooster=" + JsonConvert.SerializeObject(GameData.selectingBoosters));
    }

    private void LoadPlayerDailyQuestData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerDailyQuestData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_DAILY_QUEST_DATA).text;
            ProfileManager.UserProfile.playerDailyQuestData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerDailyQuestData;
        }

        GameData.playerDailyQuests = JsonConvert.DeserializeObject<_PlayerDailyQuestData>(s);
        DebugCustom.Log("PlayerDailyQuestData=" + JsonConvert.SerializeObject(GameData.playerDailyQuests));
    }

    private void LoadPlayerAchievementData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerAchievementData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_ACHIEVEMENT_DATA).text;
            ProfileManager.UserProfile.playerAchievementData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerAchievementData;
        }

        GameData.playerAchievements = JsonConvert.DeserializeObject<_PlayerAchievementData>(s);

        // Remove unused achievement 
        List<AchievementType> tmp = new List<AchievementType>();

        foreach (KeyValuePair<AchievementType, PlayerAchievementData> achievement in GameData.playerAchievements)
        {
            bool isNotUse = true;

            for (int i = 0; i < GameData.staticAchievementData.Count; i++)
            {
                StaticAchievementData staticData = GameData.staticAchievementData[i];

                if (staticData.type == achievement.Key)
                {
                    isNotUse = false;
                    break;
                }
            }

            if (isNotUse)
                tmp.Add(achievement.Key);
        }

        for (int i = 0; i < tmp.Count; i++)
        {
            GameData.playerAchievements.Remove(tmp[i]);
        }

        DebugCustom.Log("PlayerAchievementData=" + JsonConvert.SerializeObject(GameData.playerAchievements));
    }

    private void LoadPlayerTutorialData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerTutorialData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_TUTORIAL_DATA).text;
            ProfileManager.UserProfile.playerTutorialData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerTutorialData;
        }

        GameData.playerTutorials = JsonConvert.DeserializeObject<_PlayerTutorialData>(s);

        if (GameData.playerCampaignStageProgress.Count > 0)
        {
            GameData.playerTutorials.SetComplete(TutorialType.WorldMap);
            GameData.playerTutorials.SetComplete(TutorialType.Booster);
            GameData.playerTutorials.SetComplete(TutorialType.ActionInGame);
        }

        if (GameData.playerRambos.GetRamboLevel(ProfileManager.UserProfile.ramboId) > 1)
        {
            GameData.playerTutorials.SetComplete(TutorialType.Character);
        }

        bool isUziLevel1 = GameData.playerGuns.GetGunLevel(StaticValue.GUN_ID_UZI) == 1;
        bool isNoHaveKamePower = GameData.playerGuns.ContainsKey(StaticValue.GUN_ID_KAME_POWER) == false;

        if (!isUziLevel1 || !isNoHaveKamePower)
        {
            GameData.playerTutorials.SetComplete(TutorialType.Weapon);
        }

        DebugCustom.Log("PlayerTutorialData=" + JsonConvert.SerializeObject(GameData.playerTutorials));
    }

    #endregion


    #region IAP DATA


    #endregion

}
