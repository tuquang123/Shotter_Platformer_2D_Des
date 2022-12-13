using UnityEngine;
using System.Collections;

public class SubStepGoUpgradeWeaponFromLose : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepGoUpgradeWeaponFromLose, (sender, param) =>
        {
            Next();
        });
    }
}
