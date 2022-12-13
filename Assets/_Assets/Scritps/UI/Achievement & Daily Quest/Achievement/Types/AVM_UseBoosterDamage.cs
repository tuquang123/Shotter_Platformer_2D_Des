using UnityEngine;
using System.Collections;

public class AVM_UseBoosterDamage : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UseBoosterDamage, (sender, param) => IncreaseProgress());
    }
}
