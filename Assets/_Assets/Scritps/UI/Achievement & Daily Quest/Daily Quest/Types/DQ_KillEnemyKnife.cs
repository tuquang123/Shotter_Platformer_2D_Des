using UnityEngine;
using System.Collections;

public class DQ_KillEnemyKnife : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyKnife, (sender, param) => IncreaseProgress());
    }
}
