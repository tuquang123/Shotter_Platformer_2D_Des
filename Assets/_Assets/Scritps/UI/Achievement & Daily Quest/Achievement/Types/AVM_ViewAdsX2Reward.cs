using UnityEngine;
using System.Collections;

public class AVM_ViewAdsX2Reward : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.ViewAdsx2CoinEndGame, (sender, param) =>
        {
            IncreaseProgress();
            Save();
            GameData.playerAchievements.Save();
        });
    }
}
