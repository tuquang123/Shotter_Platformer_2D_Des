using UnityEngine;
using System.Collections;

public class SubStepUpgradeUziToLevel2 : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepUpgradeUziTolevel2, (sender, param) =>
        {
            Next();
        });
    }
}
