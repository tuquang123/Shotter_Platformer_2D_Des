using UnityEngine;
using System.Collections;

public class AVM_KillEnemyKnife : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyKnife, (sender, param) => IncreaseProgress());
    }
}
