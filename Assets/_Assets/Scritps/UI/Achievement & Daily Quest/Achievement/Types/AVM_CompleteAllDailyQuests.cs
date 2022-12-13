using UnityEngine;
using System.Collections;

public class AVM_CompleteAllDailyQuests : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.CompleteAllDailyQuests, (sender, param) =>
        {
            IncreaseProgress();
            Save();
            GameData.playerAchievements.Save();
        });
    }
}
