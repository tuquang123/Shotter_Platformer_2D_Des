using UnityEngine;
using System.Collections;

public class SubStepUpgradeUziToLevel3 : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepUpgradeUziTolevel3, (sender, param) =>
        {
            Next();
        });
    }
}
