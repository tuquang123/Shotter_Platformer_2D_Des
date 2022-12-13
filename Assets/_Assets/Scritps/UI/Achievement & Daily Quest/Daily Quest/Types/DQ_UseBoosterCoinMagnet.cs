using UnityEngine;
using System.Collections;

public class DQ_UseBoosterCoinMagnet : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UseBoosterCoinMagnet, (sender, param) => IncreaseProgress());
    }
}
