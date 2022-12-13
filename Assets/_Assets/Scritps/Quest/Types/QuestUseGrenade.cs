using UnityEngine;
using System.Collections;

public class QuestUseGrenade : BaseQuest
{
    public int requirement;

    private int useTimes;

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_USE_GRENADES;

        base.Init();

        useTimes = 0;

        EventDispatcher.Instance.RegisterListener(EventID.UseGrenade, (sender, param) =>
        {
            useTimes++;
        });
    }

    public override bool IsCompleted()
    {
        isCompleted = useTimes >= requirement;

        return isCompleted;
    }

    public override string GetDescription()
    {
        string s = string.Format(description, requirement);

        return s;
    }

    public override string GetCurrentProgress()
    {
        string s = string.Format("{0}/{1}", useTimes, requirement);

        return s;
    }
}
