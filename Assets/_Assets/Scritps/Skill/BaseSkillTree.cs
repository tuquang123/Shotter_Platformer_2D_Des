using UnityEngine;
using System.Collections;

public class BaseSkillTree : MonoBehaviour
{
    public BaseSkill activeSkill;
    public BaseSkill[] skills;

    public void Init(int ramboId)
    {
        activeSkill = null;

        SetSkillInfo(ramboId);
    }

    private void SetSkillInfo(int ramboId)
    {
        PlayerRamboSkillData skillProgress = GameData.playerRamboSkills.GetRamboSkillProgress(ramboId);

        if (skillProgress != null)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                BaseSkill skill = skills[i];
                StaticRamboSkillData skillData = GameData.staticRamboSkillData.GetData(skill.id);

                // Set level and value
                skill.level = skillProgress.GetSkillLevel(skill.id);

                if (skill.level > 0 && skill.level <= skillData.values.Length)
                {
                    skill.value = skillData.values[skill.level - 1];
                }
                else
                {
                    skill.value = -1;
                }

                // Set active skill
                if (skill.type == SkillType.Active && skill.level > 0)
                {
                    if (activeSkill == null)
                    {
                        activeSkill = skill;
                    }
                }
            }

            if (activeSkill)
            {
                UIController.Instance.SetSkillIcon(activeSkill.id);
                UIController.Instance.imgSkillBackground.gameObject.SetActive(true);
                UIController.Instance.EnableSkill(true);
            }
            else
            {
                UIController.Instance.imgSkillBackground.gameObject.SetActive(false);
            }
        }
        else
        {
            UIController.Instance.imgSkillBackground.gameObject.SetActive(false);
        }
    }
}