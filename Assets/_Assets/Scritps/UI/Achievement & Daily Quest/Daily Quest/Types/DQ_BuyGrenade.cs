using UnityEngine;
using System.Collections;

public class DQ_BuyGrenade : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.BuyGrenade, (sender, param) =>
        {
            IncreaseProgress();
            Save();
            GameData.playerDailyQuests.Save();
        });
    }
}
