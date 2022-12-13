using UnityEngine;
using System.Collections;

public class DQ_GetCriticalHit : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.GetCriticalHit, (sender, param) => IncreaseProgress());
    }
}
