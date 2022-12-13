using UnityEngine;
using System.Collections;

public class AVM_KillEnemyCrazyMode : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, (sender, param) =>
        {
            if (GameData.mode == GameMode.Campaign && GameData.currentStage.difficulty == Difficulty.Crazy)
            {
                IncreaseProgress();
            }
        });
    }
}
