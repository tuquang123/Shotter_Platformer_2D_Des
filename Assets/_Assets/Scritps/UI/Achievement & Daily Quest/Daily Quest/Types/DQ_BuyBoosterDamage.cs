using UnityEngine;
using System.Collections;

public class DQ_BuyBoosterDamage : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.BuyBooster, (sender, param) =>
        {
            if ((BoosterType)param == BoosterType.Damage)
            {
                IncreaseProgress();
                Save();
                GameData.playerDailyQuests.Save();
            }
        });
    }
}
