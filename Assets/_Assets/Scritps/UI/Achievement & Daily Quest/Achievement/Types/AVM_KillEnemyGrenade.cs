using UnityEngine;
using System.Collections;

public class AVM_KillEnemyGrenade : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyGrenade, (sender, param) => IncreaseProgress());
    }
}
