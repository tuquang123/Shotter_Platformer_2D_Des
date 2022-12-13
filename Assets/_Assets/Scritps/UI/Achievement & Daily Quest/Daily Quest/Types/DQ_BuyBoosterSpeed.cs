using UnityEngine;
using System.Collections;

public class DQ_BuyBoosterSpeed : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.BuyBooster, (sender, param) =>
        {
            if ((BoosterType)param == BoosterType.Speed)
            {
                IncreaseProgress();
                Save();
                GameData.playerDailyQuests.Save();
            }
        });
    }
}
