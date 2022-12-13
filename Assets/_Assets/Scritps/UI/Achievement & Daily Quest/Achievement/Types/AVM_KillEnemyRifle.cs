using UnityEngine;
using System.Collections;

public class AVM_KillEnemyRifle : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyRifle, (sender, param) => IncreaseProgress());
    }
}
