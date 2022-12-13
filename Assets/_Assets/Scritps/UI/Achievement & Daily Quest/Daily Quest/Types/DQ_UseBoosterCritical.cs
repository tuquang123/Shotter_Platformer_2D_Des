using UnityEngine;
using System.Collections;

public class DQ_UseBoosterCritical : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UseBoosterCritical, (sender, param) => IncreaseProgress());
    }
}
