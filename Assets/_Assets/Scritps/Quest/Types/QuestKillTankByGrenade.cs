using UnityEngine;
using System.Collections;

public class QuestKillTankByGrenade : BaseQuest
{
    public int numberTankRequirement;

    private int tankKilledByGrenade;

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_KILL_TANK_BY_GRENADE;

        base.Init();

        tankKilledByGrenade = 0;

        EventDispatcher.Instance.RegisterListener(EventID.KillTankByGrenade, (sender, param) =>
        {
            tankKilledByGrenade++;
        });
    }

    public override bool IsCompleted()
    {
        isCompleted = tankKilledByGrenade >= numberTankRequirement;

        return isCompleted;
    }

    public override string GetDescription()
    {
        string s = string.Format(description, numberTankRequirement);

        return s;
    }

    public override string GetCurrentProgress()
    {
        string s = string.Format("{0}/{1}", tankKilledByGrenade, numberTankRequirement);

        return s;
    }
}
