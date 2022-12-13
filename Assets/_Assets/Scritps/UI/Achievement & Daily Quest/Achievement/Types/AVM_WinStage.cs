using UnityEngine;
using System.Collections;

public class AVM_WinStage : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.GameEnd, (sender, param) =>
        {
            if ((bool)param)
            {
                if (GameData.mode == GameMode.Campaign)
                {
                    IncreaseProgress();
                    Save();
                }
            }
        });
    }
}
