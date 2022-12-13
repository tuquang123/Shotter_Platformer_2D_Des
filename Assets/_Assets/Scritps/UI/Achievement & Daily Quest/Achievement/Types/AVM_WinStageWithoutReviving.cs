using UnityEngine;
using System.Collections;

public class AVM_WinStageWithoutReviving : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.CompleteStageWithoutReviving, (sender, param) => IncreaseProgress());
    }
}
