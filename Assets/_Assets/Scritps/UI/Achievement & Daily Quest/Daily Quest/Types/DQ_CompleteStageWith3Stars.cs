using UnityEngine;
using System.Collections;

public class DQ_CompleteStageWith3Stars : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.FinishStageWith3Stars, (sender, param) => IncreaseProgress());
    }
}
