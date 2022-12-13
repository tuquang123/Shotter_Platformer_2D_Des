using UnityEngine;
using System.Collections;

public class AVM_UseBoosterCoinMagnet : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UseBoosterCoinMagnet, (sender, param) => IncreaseProgress());
    }
}
