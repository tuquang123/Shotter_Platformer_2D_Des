using UnityEngine;
using System.Collections;

public class DQ_BuyBoosterHP : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.BuyBooster, (sender, param) =>
        {
            if ((BoosterType)param == BoosterType.Hp)
            {
                IncreaseProgress();
                Save();
                GameData.playerDailyQuests.Save();
            }
        });
    }
}
