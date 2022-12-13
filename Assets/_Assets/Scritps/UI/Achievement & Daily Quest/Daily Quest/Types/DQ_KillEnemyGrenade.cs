using UnityEngine;
using System.Collections;

public class DQ_KillEnemyGrenade : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyGrenade, (sender, param) => IncreaseProgress());
    }
}
