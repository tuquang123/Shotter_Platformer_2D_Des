using UnityEngine;
using System.Collections;

public class DQ_CompleteStage : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.FinishStage, (sender, param) => IncreaseProgress());
    }
}
