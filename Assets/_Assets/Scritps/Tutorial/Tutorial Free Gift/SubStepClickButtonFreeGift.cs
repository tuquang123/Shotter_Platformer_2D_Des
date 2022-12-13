using UnityEngine;
using System.Collections;

public class SubStepClickButtonFreeGift : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepClickButtonFreeGift, (sender, param) =>
        {
            Next();
        });
    }
}
