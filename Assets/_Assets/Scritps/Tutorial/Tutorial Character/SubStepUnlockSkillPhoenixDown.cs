using UnityEngine;
using System.Collections;

public class SubStepUnlockSkillPhoenixDown : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepUnlockSkillPhoenixDown, (sender, param) =>
        {
            Next();
            GameData.playerTutorials.SetComplete(TutorialType.Character);
        });
    }
}
