using UnityEngine;
using System.Collections;

public class AVM_KillEnemyTank : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyTank, (sender, param) => IncreaseProgress());
    }
}
