using UnityEngine;
using System.Collections;

public class AVM_KillEnemy : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, (sender, param) => IncreaseProgress());
    }
}
