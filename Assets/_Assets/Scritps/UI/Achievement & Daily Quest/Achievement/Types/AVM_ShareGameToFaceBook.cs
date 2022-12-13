using UnityEngine;
using System.Collections;

public class AVM_ShareGameToFaceBook : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.ShareFacebookSuccess, (sender, param) =>
        {
            IncreaseProgress();
            Save();
            GameData.playerAchievements.Save();
        });
    }
}
