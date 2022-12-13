using UnityEngine;
using System.Collections;

public class SubStepSelectSkillPhoenixDown : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepSelectSkillPhoenixDown, (sender, param) =>
        {
            Next();
        });
    }
}
