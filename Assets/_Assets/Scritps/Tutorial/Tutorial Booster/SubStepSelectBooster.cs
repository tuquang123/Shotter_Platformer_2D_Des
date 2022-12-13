using UnityEngine;
using System.Collections;

public class SubStepSelectBooster : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepSelectBooster, (sender, param) =>
        {
            Next();
        });
    }
}
