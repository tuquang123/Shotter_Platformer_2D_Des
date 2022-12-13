using UnityEngine;
using System.Collections;

public class DQ_UseBoosterDamage : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UseBoosterDamage, (sender, param) => IncreaseProgress());
    }
}
