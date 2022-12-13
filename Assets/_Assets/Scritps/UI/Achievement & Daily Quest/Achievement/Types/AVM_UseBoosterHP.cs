using UnityEngine;
using System.Collections;

public class AVM_UseBoosterHP : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UseBoosterHP, (sender, param) => IncreaseProgress());
    }
}
