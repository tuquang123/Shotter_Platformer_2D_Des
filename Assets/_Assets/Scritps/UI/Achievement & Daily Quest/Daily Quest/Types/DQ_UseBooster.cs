using UnityEngine;
using System.Collections;

public class DQ_UseBooster : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UseBooster, (sender, param) => IncreaseProgress());
    }
}
