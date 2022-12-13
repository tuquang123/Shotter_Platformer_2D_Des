using UnityEngine;
using System.Collections;

public class AVM_KillEnemyGeneral : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyGeneral, (sender, param) => IncreaseProgress());
    }
}
