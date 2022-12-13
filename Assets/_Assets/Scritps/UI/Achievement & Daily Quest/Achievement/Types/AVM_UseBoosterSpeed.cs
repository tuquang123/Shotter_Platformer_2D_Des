using UnityEngine;
using System.Collections;

public class AVM_UseBoosterSpeed : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UseBoosterSpeed, (sender, param) => IncreaseProgress());
    }
}
