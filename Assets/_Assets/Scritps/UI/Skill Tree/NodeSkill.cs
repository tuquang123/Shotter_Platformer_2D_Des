using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spine.Unity;

public class NodeSkill : MonoBehaviour
{
    public Image icon;
    public Text textLevel;
    public GameObject notiCanLearn;
    public GameObject highlight;
    public SkeletonGraphic effectUpgrade;

    private int id;
    private int level;


    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.UpgradeSkillSuccess, (sender, param) => OnUpgradeSkillSuccess((int)param));
        EventDispatcher.Instance.RegisterListener(EventID.ResetUISkillTree, (sender, param) => ActiveHighlight(false));
        EventDispatcher.Instance.RegisterListener(EventID.ClickNodeSkill, (sender, param) =>
        {
            ActiveHighlight((int)param == id);
        });

        effectUpgrade.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ActiveHighlight(false);
    }

    public void Load(int id, int level)
    {
        this.id = id;
        this.level = level;

        icon.sprite = level > 0 ? GameResourcesUtils.GetSkillUnlockImage(id) : GameResourcesUtils.GetSkillLockImage(id);
        icon.SetNativeSize();

        textLevel.text = level.ToString();
        textLevel.transform.parent.gameObject.SetActive(level > 0);

        StaticRamboSkillData staticData = GameData.staticRamboSkillData.GetData(id);

        if (staticData != null)
        {
            int requireSkill = staticData.requireSkillId;
        }

        if (level > 0)
        {
            notiCanLearn.SetActive(false);
        }
        else
        {
            int unUsedSkillPoints = GameData.playerRamboSkills.GetUnusedSkillPoints(staticData.ramboId);

            if (unUsedSkillPoints <= 0)
            {
                notiCanLearn.SetActive(false);
            }
            else
            {
                PlayerRamboSkillData progress = GameData.playerRamboSkills.GetRamboSkillProgress(staticData.ramboId);

                if (staticData.isRequirePreviousSkill == false || progress.GetSkillLevel(staticData.requireSkillId) > 0)
                {
                    notiCanLearn.SetActive(true);
                }
                else
                {
                    notiCanLearn.SetActive(false);
                }
            }
        }
    }

    public void OnClick()
    {
        EventDispatcher.Instance.PostEvent(EventID.ClickNodeSkill, id);

        if (GameData.isShowingTutorial)
        {
            // Skill phoenix down
            if (id == 6)
            {
                EventDispatcher.Instance.PostEvent(EventID.SubStepSelectSkillPhoenixDown);
            }
        }
    }

    private void OnUpgradeSkillSuccess(int id)
    {
        if (this.id == id)
        {
            effectUpgrade.gameObject.SetActive(true);
            effectUpgrade.AnimationState.SetAnimation(0, "animation", false);
        }
    }

    private void ActiveHighlight(bool isActive)
    {
        highlight.SetActive(isActive);
    }
}
