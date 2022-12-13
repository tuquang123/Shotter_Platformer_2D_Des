using UnityEngine;
using System.Collections;

public class DQ_ShareFacebook : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.ShareFacebookSuccess, (sender, param) =>
        {
            IncreaseProgress();
            Save();
            GameData.playerDailyQuests.Save();
        });
    }
}
