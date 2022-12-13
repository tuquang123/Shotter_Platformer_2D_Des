using UnityEngine;
using System.Collections;

public class SubStepUpgradeRamboToLevel2 : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepUpgradeRamboToLevel2, (sender, param) =>
        {
            Next();
        });
    }
}
