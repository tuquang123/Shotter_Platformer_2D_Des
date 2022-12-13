using UnityEngine;
using System.Collections;

public class AVM_KillEnemyFlying : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyFlying, (sender, param) => IncreaseProgress());
    }
}
