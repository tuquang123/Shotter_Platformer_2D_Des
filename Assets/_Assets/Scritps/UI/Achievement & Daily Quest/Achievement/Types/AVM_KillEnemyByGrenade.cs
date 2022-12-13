using UnityEngine;
using System.Collections;

public class AVM_KillEnemyByGrenade : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyByGrenade, (sender, param) => IncreaseProgress());
    }
}
