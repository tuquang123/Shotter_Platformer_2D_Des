using UnityEngine;
using System.Collections;

public class AVM_UseBoosterCritical : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UseBoosterCritical, (sender, param) => IncreaseProgress());
    }
}
