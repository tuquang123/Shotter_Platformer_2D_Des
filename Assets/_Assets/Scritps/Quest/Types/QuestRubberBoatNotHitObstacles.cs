using UnityEngine;
using System.Collections;

public class QuestRubberBoatNotHitObstacles : BaseQuest
{
    private bool isFailed;

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_BOAT_NOT_HIT_OBSTACLE;

        base.Init();

        isFailed = false;
        isCompleted = true;

        EventDispatcher.Instance.RegisterListener(EventID.BoatTriggerObstacle, (sender, param) =>
        {
            if (isFailed == false)
            {
                isFailed = true;
                SetComplete(false);
            }
        });
    }
}
