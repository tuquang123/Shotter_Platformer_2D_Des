using UnityEngine;
using System.Collections;

public class DQ_KillEnemyByGrenade : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyByGrenade, (sender, param) => IncreaseProgress());
    }
}
