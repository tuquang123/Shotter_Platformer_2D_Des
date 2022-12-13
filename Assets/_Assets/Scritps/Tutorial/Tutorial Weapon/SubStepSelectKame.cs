using UnityEngine;
using System.Collections;

public class SubStepSelectKame : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepSelectKame, (sender, param) =>
        {
            Next();
        });
    }
}
