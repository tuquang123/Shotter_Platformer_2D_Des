using UnityEngine;
using System.Collections;

public class SubStepClickMission : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepClickMission, (sender, param) =>
        {
            Next();
        });
    }
}
