using UnityEngine;
using System.Collections;

public class QuestFinishStageInTime : BaseQuest
{
    public int timeRequirement;

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_FINISH_STAGE_IN_TIME;

        base.Init();
    }

    public override bool IsCompleted()
    {
        isCompleted = GameController.Instance.PlayTime <= timeRequirement;

        return isCompleted;
    }

    public override string GetDescription()
    {
        string s = string.Format(description, timeRequirement);

        return s;
    }
}
