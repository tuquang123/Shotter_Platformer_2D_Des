using UnityEngine;
using System.Collections;

public class SubStepGoUpgradeCharacterFromLose : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepGoUpgradeCharacterFromLose, (sender, param) =>
        {
            Next();
        });
    }
}
