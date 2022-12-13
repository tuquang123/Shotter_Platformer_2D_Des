using UnityEngine;
using System.Collections;

public class QuestKillEnemyByKnife : BaseQuest
{
    public int requirement;

    private int enemyKilledByKnife;

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_KILL_ENEMY_BY_KNIFE;

        base.Init();

        enemyKilledByKnife = 0;

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyByKnife, (sender, param) =>
        {
            enemyKilledByKnife++;
        });
    }

    public override bool IsCompleted()
    {
        isCompleted = enemyKilledByKnife >= requirement;

        return isCompleted;
    }

    public override string GetDescription()
    {
        string s = string.Format(description, requirement);

        return s;
    }

    public override string GetCurrentProgress()
    {
        string s = string.Format("{0}/{1}", enemyKilledByKnife, requirement);

        return s;
    }
}
