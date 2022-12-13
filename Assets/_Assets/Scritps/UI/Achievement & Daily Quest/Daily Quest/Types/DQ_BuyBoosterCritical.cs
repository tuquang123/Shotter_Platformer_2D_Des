using UnityEngine;
using System.Collections;

public class DQ_BuyBoosterCritical : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.BuyBooster, (sender, param) =>
        {
            if ((BoosterType)param == BoosterType.Critical)
            {
                IncreaseProgress();
                Save();
                GameData.playerDailyQuests.Save();
            }
        });
    }
}
