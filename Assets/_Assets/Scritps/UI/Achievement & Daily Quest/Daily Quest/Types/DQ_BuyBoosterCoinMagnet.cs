using UnityEngine;
using System.Collections;

public class DQ_BuyBoosterCoinMagnet : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.BuyBooster, (sender, param) =>
        {
            if ((BoosterType)param == BoosterType.CoinMagnet)
            {
                IncreaseProgress();
                Save();
                GameData.playerDailyQuests.Save();
            }
        });
    }
}
