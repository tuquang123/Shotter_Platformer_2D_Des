using UnityEngine;
using System.Collections;

public class DQ_KillEnemyTank : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyTank, (sender, param) => IncreaseProgress());
    }
}
