using UnityEngine;
using System.Collections;

public class DQ_KillEnemyByMeleeWeapon : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyByKnife, (sender, param) => IncreaseProgress());
    }
}
