using UnityEngine;
using System.Collections;

public class DQ_KillEnemy : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, (sender, param) => IncreaseProgress());
    }
}
