using UnityEngine;
using System.Collections;

public class DQ_UseBoosterSpeed : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UseBoosterSpeed, (sender, param) => IncreaseProgress());
    }
}
