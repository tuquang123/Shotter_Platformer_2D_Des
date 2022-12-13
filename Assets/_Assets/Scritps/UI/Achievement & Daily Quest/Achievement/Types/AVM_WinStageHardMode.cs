using UnityEngine;
using System.Collections;

public class AVM_WinStageHardMode : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.GameEnd, (sender, param) =>
        {
            if ((bool)param && GameData.mode == GameMode.Campaign && GameData.currentStage.difficulty == Difficulty.Hard)
            {
                IncreaseProgress();
                Save();
            }
        });
    }
}
