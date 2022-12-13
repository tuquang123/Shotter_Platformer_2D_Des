using UnityEngine;
using System.Collections;

public class QuestKillBossMonkeyByGrenade : BaseQuest
{

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_KILL_BOSS_MONKEY_BY_GRENADE;

        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, (sender, param) =>
        {
            UnitDieData data = (UnitDieData)param;

            if (data != null)
            {
                if (GameData.mode == GameMode.Campaign && data.attackData.weapon == WeaponType.Grenade && data.unit.id == StaticValue.ID_BOSS_MONKEY)
                {
                    SetComplete(true);
                }
            }
        });
    }
}
