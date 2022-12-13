using UnityEngine;
using System.Collections;

public class DQ_PassSurvivalWave5 : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.CompleteSurvivalWave5, (sender, param) =>
        {
            IncreaseProgress();
            Save();
            GameData.playerDailyQuests.Save();
        });
    }
}
