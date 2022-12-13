using UnityEngine;
using System.Collections;

public class QuestUseBoosterDamage : BaseQuest
{
    private int useTimes;

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_KILL_ENEMY_BY_KNIFE;

        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UseBoosterDamage, (sender, param) =>
        {
            SetComplete(true);
        });
    }
}
