using UnityEngine;
using System.Collections;

public class DQ_CompleteAllQuests : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.CompleteAllDailyQuests, (sender, param) => IncreaseProgress());
    }
}
