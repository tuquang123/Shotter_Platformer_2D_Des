using UnityEngine;
using System.Collections;

public class AVM_GetFreeCoin : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.ViewAdsGetFreeCoin, (sender, param) =>
        {
            IncreaseProgress();
            Save();
            GameData.playerAchievements.Save();
        });
    }
}
