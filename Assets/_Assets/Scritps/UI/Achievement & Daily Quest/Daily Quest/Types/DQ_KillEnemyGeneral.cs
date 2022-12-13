using UnityEngine;
using System.Collections;

public class DQ_KillEnemyGeneral : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyGeneral, (sender, param) => IncreaseProgress());
    }
}
