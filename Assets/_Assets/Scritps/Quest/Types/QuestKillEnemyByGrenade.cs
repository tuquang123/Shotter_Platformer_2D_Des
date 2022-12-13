using UnityEngine;
using System.Collections;

public class QuestKillEnemyByGrenade : BaseQuest
{
    public int requirement;

    private int enemyKilledByGrenade;

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_KILL_ENEMY_BY_GRENADE;

        base.Init();

        enemyKilledByGrenade = 0;

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyByGrenade, (sender, param) =>
        {
            enemyKilledByGrenade++;
        });
    }

    public override bool IsCompleted()
    {
        isCompleted = enemyKilledByGrenade >= requirement;

        return isCompleted;
    }

    public override string GetDescription()
    {
        string s = string.Format(description, requirement);

        return s;
    }

    public override string GetCurrentProgress()
    {
        string s = string.Format("{0}/{1}", enemyKilledByGrenade, requirement);

        return s;
    }
}
