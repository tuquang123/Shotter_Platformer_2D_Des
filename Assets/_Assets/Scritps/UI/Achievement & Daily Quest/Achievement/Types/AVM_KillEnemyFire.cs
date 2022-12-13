using UnityEngine;
using System.Collections;

public class AVM_KillEnemyFire : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyFire, (sender, param) => IncreaseProgress());
    }
}
