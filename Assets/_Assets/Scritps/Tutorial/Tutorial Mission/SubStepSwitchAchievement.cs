using UnityEngine;
using System.Collections;

public class SubStepSwitchAchievement : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepClickAchievement, (sender, param) =>
        {
            Next();
        });
    }
}
