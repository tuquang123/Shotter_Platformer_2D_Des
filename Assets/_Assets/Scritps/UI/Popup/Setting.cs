using Facebook.Unity;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Text textVersion;
    public Slider sound;
    public Slider music;

    void OnEnable()
    {
        if (FB.IsLoggedIn)
        {
            textVersion.text = "ID: " + AccessToken.CurrentAccessToken.UserId;
        }

        sound.value = ProfileManager.UserProfile.soundVolume;
        music.value = ProfileManager.UserProfile.musicVolume;
    }

    void OnDisable()
    {
        ProfileManager.UserProfile.soundVolume.Set(sound.value);
        ProfileManager.UserProfile.musicVolume.Set(music.value);
        ProfileManager.SaveAll();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnAdjustSoundVolume()
    {
        SoundManager.Instance.AdjustSoundVolume(sound.value);
    }

    public void OnAdjustMusicVolume()
    {
        SoundManager.Instance.AdjustMusicVolume(music.value);
    }

    public void BackUpData()
    {
        Hide();

        Popup.Instance.Show("Do you want to save current game data?", "CONFIRMATION", PopupType.YesNo, () =>
        {
            if (FB.IsLoggedIn)
            {
                ProcessBackupData();
            }
            else
            {
                FbController.Instance.LoginWithReadPermission(success =>
                {
                    if (success)
                    {
                        ProcessBackupData();
                    }
                    else
                    {
                        Popup.Instance.ShowToastMessage("Login Facebook failed", ToastLength.Long);
                    }
                });
            }
        });
    }

    public void AutoBackupData()
    {
        if (FB.IsLoggedIn)
        {
            string fbId = AccessToken.CurrentAccessToken.UserId;

            FireBaseDatabase.Instance.AuthenWithFacebook(fbId, AccessToken.CurrentAccessToken.TokenString, authUser =>
            {
                if (authUser != null)
                {
                    FireBaseDatabase.Instance.SaveUserData(fbId, complete =>
                    {
                        DebugCustom.Log("Auto backup data: " + complete);
                    });
                }
            });
        }
    }

    public void RestoreData()
    {
        Hide();

        Popup.Instance.Show(
            "Do you want to replace current game data with previous one?",
            "CONFIRMATION",
            PopupType.YesNo,
            yesCallback: () =>
            {
                Popup.Instance.ShowInstantLoading();

                if (FB.IsLoggedIn)
                {
                    ProcessRestoreData();
                }
                else
                {
                    FbController.Instance.LoginWithReadPermission(success =>
                    {
                        if (success)
                        {
                            ProcessRestoreData();
                        }
                        else
                        {
                            Popup.Instance.HideInstantLoading();
                            Popup.Instance.ShowToastMessage("Login Facebook failed", ToastLength.Long);
                        }
                    });
                }
            },
            noCallback: () =>
            {
                Show();
            });
    }

    private void ProcessBackupData()
    {
        string fbId = AccessToken.CurrentAccessToken.UserId;

        FireBaseDatabase.Instance.AuthenWithFacebook(fbId, AccessToken.CurrentAccessToken.TokenString, authUser =>
        {
            if (authUser != null)
            {
                FireBaseDatabase.Instance.SaveUserData(fbId, complete =>
                {
                    if (complete)
                    {
                        Popup.Instance.ShowToastMessage("Save game data successfully", ToastLength.Long);
                    }
                    else
                    {
                        Popup.Instance.ShowToastMessage("Save game data error, please try again later. Sorry for this inconvinient", ToastLength.Long);
                    }
                });
            }
            else
            {
                Popup.Instance.ShowToastMessage("Authentication failed", ToastLength.Long);
            }
        });
    }

    private void ProcessRestoreData()
    {
        string fbId = AccessToken.CurrentAccessToken.UserId;

        FireBaseDatabase.Instance.AuthenWithFacebook(fbId, AccessToken.CurrentAccessToken.TokenString, authUser =>
        {
            if (authUser != null)
            {
                FireBaseDatabase.Instance.GetUserData(fbId, inventory =>
                {
                    Popup.Instance.HideInstantLoading();

                    if (inventory != null)
                    {
                        ProfileManager.Load(inventory);
                        ProfileManager.SaveAll();
                        SceneManager.LoadScene(StaticValue.SCENE_LOGIN);
                    }
                    else
                    {
                        DebugCustom.Log("Inventory empty");
                    }
                });
            }
            else
            {
                Popup.Instance.HideInstantLoading();
                Popup.Instance.ShowToastMessage("Authentication failed", ToastLength.Long);
            }
        });
    }


    #region CHEATING FOR TEST

    public void MaxData()
    {
        string s = Resources.Load<TextAsset>("JSON/TMP/max_campaign_progress").text;
        ProfileManager.UserProfile.playerCampaignProgessData.Set(string.Empty);
        ProfileManager.UserProfile.playerCampaignStageProgessData.Set(s);
        GameData.playerCampaignStageProgress = JsonConvert.DeserializeObject<_PlayerCampaignStageProgressData>(s);
        DebugCustom.Log("MaxPlayerCampaignProgess=" + JsonConvert.SerializeObject(GameData.playerCampaignStageProgress));

        s = Resources.Load<TextAsset>("JSON/TMP/max_campaign_reward_progress").text;
        ProfileManager.UserProfile.playerCampaignRewardProgessData.Set(s);
        GameData.playerCampaignRewardProgress = JsonConvert.DeserializeObject<_PlayerCampaignRewardProgressData>(s);
        DebugCustom.Log("MaxPlayerCampaignRewardProgess=" + JsonConvert.SerializeObject(GameData.playerCampaignRewardProgress));

        s = Resources.Load<TextAsset>("JSON/TMP/max_grenade_data").text;
        ProfileManager.UserProfile.playerGrenadeData.Set(s);
        GameData.playerGrenades = JsonConvert.DeserializeObject<_PlayerGrenadeData>(s);
        DebugCustom.Log("MaxPlayerGrenadeData=" + JsonConvert.SerializeObject(GameData.playerGrenades));

        s = Resources.Load<TextAsset>("JSON/TMP/max_gun_data").text;
        ProfileManager.UserProfile.playerGunData.Set(s);
        GameData.playerGuns = JsonConvert.DeserializeObject<_PlayerGunData>(s);
        DebugCustom.Log("MaxPlayerGunData=" + JsonConvert.SerializeObject(GameData.playerGuns));

        s = Resources.Load<TextAsset>("JSON/TMP/max_melee_weapon_data").text;
        ProfileManager.UserProfile.playerMeleeWeaponData.Set(s);
        GameData.playerMeleeWeapons = JsonConvert.DeserializeObject<_PlayerMeleeWeaponData>(s);
        DebugCustom.Log("MaxPlayerMeleeWeaponData=" + JsonConvert.SerializeObject(GameData.playerMeleeWeapons));

        s = Resources.Load<TextAsset>("JSON/TMP/max_resources_data").text;
        ProfileManager.UserProfile.playerResourcesData.Set(s);
        GameData.playerResources = JsonConvert.DeserializeObject<_PlayerResourcesData>(s);
        DebugCustom.Log("MaxPlayerResourcesData=" + JsonConvert.SerializeObject(GameData.playerResources));

        s = Resources.Load<TextAsset>("JSON/TMP/max_rambo_data").text;
        ProfileManager.UserProfile.playerRamboData.Set(s);
        GameData.playerRambos = JsonConvert.DeserializeObject<_PlayerRamboData>(s);
        DebugCustom.Log("MaxPlayerRamboData=" + JsonConvert.SerializeObject(GameData.playerRambos));

        s = Resources.Load<TextAsset>("JSON/TMP/max_booster_data").text;
        ProfileManager.UserProfile.playerBoosterData.Set(s);
        GameData.playerBoosters = JsonConvert.DeserializeObject<_PlayerBoosterData>(s);
        DebugCustom.Log("MaxPlayerBoosterData=" + JsonConvert.SerializeObject(GameData.playerBoosters));

        ProfileManager.SaveAll();

        gameObject.SetActive(false);
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU, false);
    }

    public void ResetData()
    {
        ProfileManager.DeleteAll();

        string s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_RESOURCES_DATA).text;
        ProfileManager.UserProfile.playerResourcesData.Set(s);
        GameData.playerResources = JsonConvert.DeserializeObject<_PlayerResourcesData>(s);
        DebugCustom.Log("PlayerResources=" + JsonConvert.SerializeObject(GameData.playerResources));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_RAMBO_DATA).text;
        ProfileManager.UserProfile.playerRamboData.Set(s);
        ProfileManager.UserProfile.ramboId.Set(StaticValue.RAMBO_ID_JOHN);
        GameData.playerRambos = JsonConvert.DeserializeObject<_PlayerRamboData>(s);
        DebugCustom.Log("PlayerRambos=" + JsonConvert.SerializeObject(GameData.playerRambos));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_GUN_DATA).text;
        ProfileManager.UserProfile.playerGunData.Set(s);
        ProfileManager.UserProfile.gunNormalId.Set(StaticValue.GUN_ID_UZI);
        ProfileManager.UserProfile.gunSpecialId.Set(-1);
        GameData.playerGuns = JsonConvert.DeserializeObject<_PlayerGunData>(s);
        DebugCustom.Log("PlayerGuns=" + JsonConvert.SerializeObject(GameData.playerGuns));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_GRENADE_DATA).text;
        ProfileManager.UserProfile.playerGrenadeData.Set(s);
        ProfileManager.UserProfile.grenadeId.Set(StaticValue.GRENADE_ID_F1);
        GameData.playerGrenades = JsonConvert.DeserializeObject<_PlayerGrenadeData>(s);
        DebugCustom.Log("PlayerGrenades=" + JsonConvert.SerializeObject(GameData.playerGrenades));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_MELEE_WEAPON_DATA).text;
        ProfileManager.UserProfile.playerMeleeWeaponData.Set(s);
        ProfileManager.UserProfile.meleeWeaponId.Set(StaticValue.MELEE_WEAPON_ID_KNIFE);
        GameData.playerMeleeWeapons = JsonConvert.DeserializeObject<_PlayerMeleeWeaponData>(s);
        DebugCustom.Log("PlayerMeleeWeapons=" + JsonConvert.SerializeObject(GameData.playerMeleeWeapons));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_CAMPAIGN_STAGE_PROGRESS_DATA).text;
        ProfileManager.UserProfile.playerCampaignStageProgessData.Set(s);
        ProfileManager.UserProfile.playerCampaignProgessData.Set(string.Empty);
        GameData.playerCampaignStageProgress = JsonConvert.DeserializeObject<_PlayerCampaignStageProgressData>(s);
        DebugCustom.Log("PlayerCampaignProgress=" + JsonConvert.SerializeObject(GameData.playerCampaignStageProgress));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_CAMPAIGN_REWARD_PROGRESS_DATA).text;
        ProfileManager.UserProfile.playerCampaignRewardProgessData.Set(s);
        GameData.playerCampaignRewardProgress = JsonConvert.DeserializeObject<_PlayerCampaignRewardProgressData>(s);
        DebugCustom.Log("PlayerCampaignRewardProgress=" + JsonConvert.SerializeObject(GameData.playerCampaignRewardProgress));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_BOOSTER_DATA).text;
        ProfileManager.UserProfile.playerBoosterData.Set(s);
        GameData.playerBoosters = JsonConvert.DeserializeObject<_PlayerBoosterData>(s);
        DebugCustom.Log("PlayerBoosters=" + JsonConvert.SerializeObject(GameData.playerBoosters));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_SELECTING_BOOSTER).text;
        ProfileManager.UserProfile.playerSelectingBooster.Set(s);
        GameData.selectingBoosters = JsonConvert.DeserializeObject<_PlayerSelectingBooster>(s);
        DebugCustom.Log("PlayerSelectingBooster=" + JsonConvert.SerializeObject(GameData.selectingBoosters));

        ProfileManager.SaveAll();

        gameObject.SetActive(false);
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU, false);
    }

    #endregion
}
