using UnityEngine;
using System.Collections;

public class SubStepSwitchSpecialTab : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepSwitchSpecialTab, (sender, param) =>
        {
            Next();
        });
    }
}
