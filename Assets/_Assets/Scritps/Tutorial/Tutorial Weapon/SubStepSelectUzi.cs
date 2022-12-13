using UnityEngine;
using System.Collections;

public class SubStepSelectUzi : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepSelectUzi, (sender, param) =>
        {
            Next();
        });
    }
}
