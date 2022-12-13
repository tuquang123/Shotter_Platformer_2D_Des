using UnityEngine;
using System.Collections;

public class AVM_PlayStage : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.GameEnd, (sender, param) =>
        {
            if (GameData.mode == GameMode.Campaign)
            {
                IncreaseProgress();
                Save();
            }
        });
    }
}
