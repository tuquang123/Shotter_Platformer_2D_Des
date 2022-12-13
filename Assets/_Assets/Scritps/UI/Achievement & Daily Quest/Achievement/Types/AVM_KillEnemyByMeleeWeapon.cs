using UnityEngine;
using System.Collections;

public class AVM_KillEnemyByMeleeWeapon : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyByKnife, (sender, param) => IncreaseProgress());
    }
}
