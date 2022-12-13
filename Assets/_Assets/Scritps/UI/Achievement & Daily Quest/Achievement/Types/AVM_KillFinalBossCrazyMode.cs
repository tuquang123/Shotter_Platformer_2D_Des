using UnityEngine;
using System.Collections;

public class AVM_KillFinalBossCrazyMode : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.FinalBossDie, (sender, param) =>
        {
            if (GameData.mode == GameMode.Campaign && GameData.currentStage.difficulty == Difficulty.Crazy)
            {
                IncreaseProgress();
            }
        });
    }
}
