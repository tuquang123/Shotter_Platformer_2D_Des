using UnityEngine;
using System.Collections;

public class QuestFinishStageWithoutRevive : BaseQuest
{
    private bool isFailed;

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_FINISH_STAGE_WITHOUT_REVIVE;

        base.Init();

        isFailed = false;
        isCompleted = true;

        EventDispatcher.Instance.RegisterListener(EventID.ReviveByGem, (sender, param) =>
        {
            if (isFailed == false)
            {
                isFailed = true;
                SetComplete(false);
            }
        });

        EventDispatcher.Instance.RegisterListener(EventID.ReviveByAds, (sender, param) =>
        {
            if (isFailed == false)
            {
                isFailed = true;
                SetComplete(false);
            }
        });
    }
}
