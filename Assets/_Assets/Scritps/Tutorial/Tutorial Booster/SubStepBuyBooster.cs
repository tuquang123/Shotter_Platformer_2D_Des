using UnityEngine;
using System.Collections;

public class SubStepBuyBooster : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepBuyBooster, (sender, param) =>
        {
            Next();
        });
    }
}
