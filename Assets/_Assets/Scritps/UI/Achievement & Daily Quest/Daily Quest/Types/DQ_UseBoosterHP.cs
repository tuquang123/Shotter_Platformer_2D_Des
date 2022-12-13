using UnityEngine;
using System.Collections;

public class DQ_UseBoosterHP : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UseBoosterHP, (sender, param) => IncreaseProgress());
    }
}
