using UnityEngine;
using System.Collections;

public class MainMenuAnimationEvent : MonoBehaviour
{
    public void OnAnimationComplete()
    {
        if (GameData.playerTutorials.IsCompletedStep(TutorialType.WorldMap) == false)
        {
            TutorialMenuController.Instance.ShowTutorial(TutorialType.WorldMap);
        }
        else if (GameData.playerTutorials.IsCompletedStep(TutorialType.Mission) == false)
        {
            TutorialMenuController.Instance.ShowTutorial(TutorialType.Mission);
        }
        else if (GameData.playerTutorials.IsCompletedStep(TutorialType.FreeGift) == false)
        {
            TutorialMenuController.Instance.ShowTutorial(TutorialType.FreeGift);
        }
    }

    //private void CheckTutorialWeapon()
    //{
    //    bool isUziLevel1 = GameData.playerGuns.GetGunLevel(StaticValue.GUN_ID_UZI) == 1;
    //    bool isNoHaveKamePower = GameData.playerGuns.ContainsKey(StaticValue.GUN_ID_KAME_POWER) == false;

    //    StaticGunData gunData = GameData.staticGunData.GetData(StaticValue.GUN_ID_UZI);
    //    int coinUpgradeUziLevel2 = gunData.upgradeInfo[1];
    //    int coinUpgradeUziLevel3 = gunData.upgradeInfo[2];
    //    bool isEnoughCoinUpgrade = GameData.playerResources.coin >= (coinUpgradeUziLevel2 + coinUpgradeUziLevel3);

    //    if (isUziLevel1 && isNoHaveKamePower && isEnoughCoinUpgrade)
    //    {
    //        TutorialMenuController.Instance.ShowTutorialWeapon();
    //    }
    //    else
    //    {
    //        GameData.playerTutorials.SetComplete(TutorialType.Weapon);
    //        CheckTutorialCharacter();
    //    }
    //}

    //private void CheckTutorialCharacter()
    //{
    //    if (GameData.playerRambos.GetRamboLevel(ProfileManager.UserProfile.ramboId) > 1)
    //    {
    //        GameData.playerTutorials.SetComplete(TutorialType.Character);
    //    }
    //    else
    //    {
    //        TutorialMenuController.Instance.ShowTutorialCharacter();
    //    }
    //}
}
