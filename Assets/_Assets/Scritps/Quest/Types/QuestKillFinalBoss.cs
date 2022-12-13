using UnityEngine;
using System.Collections;

public class QuestKillFinalBoss : BaseQuest
{

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_KILL_FINAL_BOSS;

        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.FinalBossDie, (sender, param) =>
        {
            SetComplete(true);
        });
    }
}
