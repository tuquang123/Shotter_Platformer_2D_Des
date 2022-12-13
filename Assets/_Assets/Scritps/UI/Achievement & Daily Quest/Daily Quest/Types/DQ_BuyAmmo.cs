using UnityEngine;
using System.Collections;

public class DQ_BuyAmmo : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.BuyAmmo, (sender, param) =>
        {
            IncreaseProgress();
            Save();
            GameData.playerDailyQuests.Save();
        });
    }
}
