using UnityEngine;
using System.Collections;

public class QuestFinishStage : BaseQuest
{

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_FINISH_STAGE;

        base.Init();
    }
}
