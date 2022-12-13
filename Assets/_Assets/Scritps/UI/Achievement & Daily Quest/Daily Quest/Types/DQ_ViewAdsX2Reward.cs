using UnityEngine;
using System.Collections;

public class DQ_ViewAdsX2Reward : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.ViewAdsx2CoinEndGame, (sender, param) =>
        {
            IncreaseProgress();
            Save();
            GameData.playerDailyQuests.Save();
        });
    }
}
