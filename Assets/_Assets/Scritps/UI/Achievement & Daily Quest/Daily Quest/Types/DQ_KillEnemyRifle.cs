using UnityEngine;
using System.Collections;

public class DQ_KillEnemyRifle : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyRifle, (sender, param) => IncreaseProgress());
    }
}
