using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class UpgradeSkillController : MonoBehaviour
{
    public static int ramboId;

    public Image ramboIcon;
    public Image skillIcon;
    public Text textGuide;
    //public Text textSkillName;
    public TMP_Text textSkillName;
    public Text textSkillDescriptionMain;
    public Text textSkillDescriptionSub;
    public Text textPriceUpgrade;
    public Text textSkillPoint;
    public Text textPriceResetSkill;

    public Button btnTrain;
    public Button btnReset;
    public Button btnUpgrade;

    public NodeSkill[] nodeOffense;
    public NodeSkill[] nodeDefense;
    public NodeSkill[] nodeUtility;

    private int selectingSkillId = -1;
    private int priceUpgrade;
    private int priceReset;
    private int remainingPoints;


    void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ClickNodeSkill, (sender, param) => OnSelectNodeSkill((int)param));
    }

    void OnEnable()
    {
        ResetUI();
        CalculatePoints();
        LoadSkillTree();
    }

    public void ClickRamboIcon()
    {
        if (selectingSkillId == -1)
            return;

        ResetUI();
    }

    public void TrainSkill()
    {
        if (remainingPoints <= 0)
        {
            SoundManager.Instance.PlaySfxClick();
            Popup.Instance.ShowToastMessage("not enough skill point");
        }
        else
        {
            remainingPoints--;
            SetTextSkillPoints();
            UpgradeSkillSuccess();
        }
    }

    public void UpgradeSkill()
    {
        if (GameData.playerResources.gem < priceUpgrade)
        {
            Popup.Instance.ShowToastMessage("Not enough gems");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeGem(priceUpgrade);
            UpgradeSkillSuccess();
        }
    }

    public void ResetPoints()
    {
        SoundManager.Instance.PlaySfxClick();

        if (GameData.playerResources.gem < GetCostResetSkill())
        {
            Popup.Instance.ShowToastMessage("Not enough gems");
            return;
        }

        int usedPoints = GameData.playerRamboSkills.GetUsedSkillPoints(ramboId);

        if (usedPoints <= 0)
        {
            Popup.Instance.ShowToastMessage("you did not spend any skill points");
        }
        else
        {
            Popup.Instance.Show(
                content: string.Format("Use <color=yellow>{0} gems</color> to reset skill points?\nGems spent on upgrading are non-refundable", GetCostResetSkill()),
                title: "reset skill",
                type: PopupType.YesNo,
                yesCallback: ResetPointsSuccess);

            FirebaseAnalyticsHelper.LogEvent("N_ResetSkillPoints", usedPoints);
        }
    }

    private void ResetPointsSuccess()
    {
        GameData.playerResources.ConsumeGem(GetCostResetSkill());
        GameData.playerRamboSkills[ramboId].Reset();
        CalculatePoints();
        LoadSkillTree();
        ResetUI();
    }

    private void UpgradeSkillSuccess()
    {
        GameData.playerRamboSkills[ramboId].IncreaseLevel(selectingSkillId);
        LoadSkillTree();
        UpdateInformation();
        UpdateResetSkillCost();

        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_UPGRADE_SUCCESS);

        EventDispatcher.Instance.PostEvent(EventID.UpgradeSkillSuccess, selectingSkillId);

        if (GameData.isShowingTutorial)
        {
            // Unlock skill Phoenix Down
            if (selectingSkillId == 6 && GameData.playerRamboSkills[ramboId].GetSkillLevel(selectingSkillId) == 1)
            {
                EventDispatcher.Instance.PostEvent(EventID.SubStepUnlockSkillPhoenixDown);
            }
        }

        StaticRamboSkillData skillData = GameData.staticRamboSkillData.GetData(selectingSkillId);
        if (skillData != null)
        {
            FirebaseAnalyticsHelper.LogEvent("N_UpgradeSkill", skillData.skillName);

            if (skillData.type == SkillType.Active && GameData.playerRamboSkills.GetRamboSkillProgress(ramboId).GetSkillLevel(selectingSkillId) == 1)
            {
                FirebaseAnalyticsHelper.LogEvent("N_UnlockActiveSkill", skillData.catergory.ToString());
            }
        }

    }

    private void ResetUI()
    {
        selectingSkillId = -1;
        priceUpgrade = 0;
        skillIcon.sprite = ramboIcon.sprite;
        skillIcon.SetNativeSize();
        textGuide.gameObject.SetActive(true);
        textSkillName.text = string.Empty;
        textSkillDescriptionMain.text = string.Empty;
        textSkillDescriptionSub.text = string.Empty;
        UpdateResetSkillCost();

        btnTrain.gameObject.SetActive(false);
        btnUpgrade.gameObject.SetActive(false);

        EventDispatcher.Instance.PostEvent(EventID.ResetUISkillTree);
    }

    private void CalculatePoints()
    {
        int ramboLevel = GameData.playerRambos.GetRamboLevel(ramboId);
        int usedSkillPoints = GameData.playerRamboSkills.GetUsedSkillPoints(ramboId);

        remainingPoints = Mathf.Clamp(ramboLevel - 1 - usedSkillPoints, 0, ramboLevel - 1);

        SetTextSkillPoints();
    }

    private void SetTextSkillPoints()
    {
        textSkillPoint.text = string.Format("POINTS:  <color=yellow>{0}</color>", remainingPoints);
    }

    private void LoadSkillTree()
    {
        PlayerRamboSkillData progress = GameData.playerRamboSkills.GetRamboSkillProgress(ramboId);

        LoadSkillOffense(progress);
        LoadSkillDefense(progress);
        LoadSkillUtility(progress);
    }

    private void LoadSkillOffense(PlayerRamboSkillData progress)
    {
        List<int> ids = new List<int>();

        for (int i = 0; i < GameData.staticRamboSkillData.Count; i++)
        {
            StaticRamboSkillData data = GameData.staticRamboSkillData[i];

            if (data.ramboId == ramboId && data.catergory == SkillCatergory.Offense)
            {
                ids.Add(data.id);
            }
        }

        ids.Sort();

        for (int i = 0; i < nodeOffense.Length; i++)
        {
            int id = ids[i];
            int level = progress[id];

            nodeOffense[i].Load(id, level);
        }
    }

    private void LoadSkillDefense(PlayerRamboSkillData progress)
    {
        List<int> ids = new List<int>();

        for (int i = 0; i < GameData.staticRamboSkillData.Count; i++)
        {
            StaticRamboSkillData data = GameData.staticRamboSkillData[i];

            if (data.ramboId == ramboId && data.catergory == SkillCatergory.Defense)
            {
                ids.Add(data.id);
            }
        }

        ids.Sort();

        for (int i = 0; i < nodeDefense.Length; i++)
        {
            int id = ids[i];
            int level = progress[id];

            nodeDefense[i].Load(id, level);
        }
    }

    private void LoadSkillUtility(PlayerRamboSkillData progress)
    {
        List<int> ids = new List<int>();

        for (int i = 0; i < GameData.staticRamboSkillData.Count; i++)
        {
            StaticRamboSkillData data = GameData.staticRamboSkillData[i];

            if (data.ramboId == ramboId && data.catergory == SkillCatergory.Utility)
            {
                ids.Add(data.id);
            }
        }

        ids.Sort();

        for (int i = 0; i < nodeUtility.Length; i++)
        {
            int id = ids[i];
            int level = progress[id];

            nodeUtility[i].Load(id, level);
        }
    }

    private void UpdateResetSkillCost()
    {
        int cost = GetCostResetSkill();
        textPriceResetSkill.text = cost.ToString("n0");
        textPriceResetSkill.color = GameData.playerResources.gem >= cost ? Color.white : StaticValue.colorNotEnoughMoney;
    }

    private void UpdateInformation()
    {
        StaticRamboSkillData staticData = GameData.staticRamboSkillData.GetData(selectingSkillId);

        if (staticData != null)
        {
            PlayerRamboSkillData progress = GameData.playerRamboSkills.GetRamboSkillProgress(ramboId);
            int level = progress.GetSkillLevel(selectingSkillId);
            bool isMaxLevel = level >= staticData.maxLevel;

            textSkillName.text = staticData.skillName.ToUpper();
            skillIcon.sprite = GameResourcesUtils.GetSkillUnlockImage(selectingSkillId);
            skillIcon.SetNativeSize();


            string subDesc = staticData.descriptionSub;

            for (int i = 0; i < staticData.values.Length; i++)
            {
                float value = staticData.values[i];

                if (level == i + 1)
                    subDesc += string.Format("<color=green>{0}</color>", value);
                else
                    subDesc += value.ToString();

                if (i < staticData.values.Length - 1)
                    subDesc += "/";
            }

            textSkillDescriptionSub.text = subDesc;

            if (level > 0)
            {
                textSkillDescriptionMain.text = string.Format(staticData.descriptionMain, staticData.values[level - 1]);

                btnTrain.gameObject.SetActive(false);
                btnUpgrade.gameObject.SetActive(!isMaxLevel);

                if (!isMaxLevel)
                {
                    priceUpgrade = staticData.upgradePrice[level];
                    textPriceUpgrade.text = priceUpgrade.ToString("n0");
                    textPriceUpgrade.color = GameData.playerResources.gem >= priceUpgrade ? Color.white : StaticValue.colorNotEnoughMoney;
                }
            }
            else
            {
                textSkillDescriptionMain.text = string.Format(staticData.descriptionMain, staticData.values[0]);

                bool isCanTrain = staticData.isRequirePreviousSkill == false || progress.GetSkillLevel(staticData.requireSkillId) > 0;
                btnTrain.gameObject.SetActive(isCanTrain);
                btnUpgrade.gameObject.SetActive(false);
            }
        }
    }

    private int GetCostResetSkill()
    {
        int pointUsed = GameData.playerRamboSkills.GetUsedSkillPoints(ramboId);
        int cost = pointUsed * StaticValue.COST_GEM_PER_POINT_RESET;

        return cost;
    }

    private void OnSelectNodeSkill(int id)
    {
        if (id == selectingSkillId)
            return;

        selectingSkillId = id;
        UpdateInformation();
        textGuide.gameObject.SetActive(false);
    }
}
