using UnityEngine;
using System.Collections;

public class AVM_UseGrenadeKillEnemyAtOnce : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.GrenadeKillEnemyAtOnce, (sender, param) =>
        {
            if ((int)param >= 3)
            {
                IncreaseProgress();
            }
        });
    }
}
