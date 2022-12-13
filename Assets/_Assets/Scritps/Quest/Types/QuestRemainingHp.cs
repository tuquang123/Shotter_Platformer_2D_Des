using UnityEngine;
using System.Collections;

public class QuestRemainingHp : BaseQuest
{
    public float hpPercentRequirement;

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_REMAINING_HP;

        base.Init();
    }

    public override bool IsCompleted()
    {
        isCompleted = GameController.Instance.Player.HpPercent >= (hpPercentRequirement / 100f);

        return isCompleted;
    }

    public override string GetDescription()
    {
        string s = string.Format(description, hpPercentRequirement);

        return s;
    }
}
