using UnityEngine;
using System.Collections;

public class AVM_KillEnemySniper : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemySniper, (sender, param) => IncreaseProgress());
    }
}
