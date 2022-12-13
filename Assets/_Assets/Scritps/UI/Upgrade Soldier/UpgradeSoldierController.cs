using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Collections.Generic;

public class UpgradeSoldierController : MonoBehaviour
{
    public Text textRamboName;
    public Text textRamboLevel;

    public Text textCurHp;
    public Text textMaxHp;

    public Text textCurSpeed;
    public Text textMaxSpeed;

    public Text textCoinUpgrade;
    public GameObject notification;

    public Color32 colorNormal;
    public Color32 colorMax;

    private int requireCoinUpgrade;

    public int SelectingRamboId { get; set; }

    private void OnEnable()
    {
        SelectingRamboId = ProfileManager.UserProfile.ramboId;

        UpdateRamboInfomation();
        CheckTutorial();
    }

    private void CheckTutorial()
    {
        if (GameData.playerTutorials.IsCompletedStep(TutorialType.Character) == false)
        {
            StaticRamboData rambo = GameData.staticRamboData.GetData(ProfileManager.UserProfile.ramboId);
            int costUpgradeLevel2 = rambo.upgradeInfo[1];

            if (GameData.playerResources.coin >= costUpgradeLevel2)
            {
                TutorialMenuController.Instance.ShowTutorial(TutorialType.Character);
            }
            else
            {
                GameData.playerTutorials.SetComplete(TutorialType.Character);
            }
        }
    }

    public void UpgradeRambo()
    {
        if (GameData.playerResources.coin < requireCoinUpgrade)
        {
            Popup.Instance.ShowToastMessage("Not enough coins");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeCoin(requireCoinUpgrade);
            UpgradeRamboSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_UPGRADE_SUCCESS);
        }
    }

    private void UpgradeRamboSuccess()
    {
        GameData.playerRambos.IncreaseRamboLevel(SelectingRamboId);
        UpdateRamboInfomation();

        if (GameData.isShowingTutorial && GameData.playerRambos.GetRamboLevel(SelectingRamboId) == 2)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepUpgradeRamboToLevel2);
        }

        FirebaseAnalyticsHelper.LogEvent("N_UpgradeRambo", "ToLevel=" + GameData.playerRambos[SelectingRamboId].level);
    }

    private void UpdateRamboInfomation()
    {
        StaticRamboData staticRamboData = GameData.staticRamboData.GetData(SelectingRamboId);

        textRamboName.text = staticRamboData.ramboName;

        if (GameData.playerRambos.ContainsKey(SelectingRamboId))
        {
            int level = GameData.playerRambos.GetRamboLevel(SelectingRamboId);
            bool isMaxLevel = level >= staticRamboData.upgradeInfo.Length;

            string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_RAMBO, SelectingRamboId, level);
            SO_BaseUnitStats curLevelStats = Resources.Load<SO_BaseUnitStats>(path);

            path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_RAMBO, SelectingRamboId, staticRamboData.upgradeInfo.Length);
            SO_BaseUnitStats maxLevelStats = Resources.Load<SO_BaseUnitStats>(path);

            textRamboLevel.gameObject.SetActive(true);
            textRamboLevel.text = string.Format("Level {0}", level);

            textCurHp.text = string.Format("{0:n0}", curLevelStats.HP * 10f);
            textMaxHp.text = string.Format("{0:n0}", maxLevelStats.HP * 10f);
            textCurHp.color = isMaxLevel ? colorMax : colorNormal;

            textCurSpeed.text = (curLevelStats.MoveSpeed * 100f).ToString("n0");
            textMaxSpeed.text = (maxLevelStats.MoveSpeed * 100f).ToString("n0");
            textCurSpeed.color = isMaxLevel ? colorMax : colorNormal;

            textCoinUpgrade.transform.parent.gameObject.SetActive(!isMaxLevel);

            if (!isMaxLevel)
            {
                requireCoinUpgrade = staticRamboData.upgradeInfo[level];
                textCoinUpgrade.text = requireCoinUpgrade.ToString("n0");
                textCoinUpgrade.color = GameData.playerResources.coin >= requireCoinUpgrade ? Color.white : StaticValue.colorNotEnoughMoney;
            }
        }

        CheckNotification();
    }

    private void CheckNotification()
    {
        int unusedPoints = GameData.playerRamboSkills.GetUnusedSkillPoints(SelectingRamboId);

        notification.SetActive(unusedPoints > 0);
    }
}
