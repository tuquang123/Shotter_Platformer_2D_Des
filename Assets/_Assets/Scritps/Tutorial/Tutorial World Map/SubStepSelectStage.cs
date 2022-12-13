using UnityEngine;
using System.Collections;

public class SubStepSelectStage : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepSelectStage, (sender, param) =>
        {
            Next();
        });
    }
}
