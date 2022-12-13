using UnityEngine;
using System.Collections;

public class AVM_KillFinalBoss : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.FinalBossDie, (sender, param) =>
        {
            if (GameData.mode == GameMode.Campaign)
            {
                IncreaseProgress();
            }
        });
    }
}
