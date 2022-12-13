using UnityEngine;
using System.Collections;

public class SubStepEnterSkillTree : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepEnterSkillTree, (sender, param) =>
        {
            Next();
        });
    }
}
