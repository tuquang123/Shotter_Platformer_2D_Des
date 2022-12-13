using UnityEngine;
using System.Collections;

public class DQ_PlayTournament : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.QuitSurvivalSession, (sender, param) =>
        {
            IncreaseProgress();
            Save();
            GameData.playerDailyQuests.Save();
        });

        EventDispatcher.Instance.RegisterListener(EventID.CompleteSurvivalSession, (sender, param) =>
        {
            IncreaseProgress();
            Save();
            GameData.playerDailyQuests.Save();
        });
    }
}
