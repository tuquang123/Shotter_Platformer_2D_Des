using UnityEngine;
using System.Collections;

public class QuestKillEnemyBySpecialGun : BaseQuest
{
    public int requirement;

    private int enemyKilledBySpecialGun;

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_KILL_ENEMY_BY_SPECIAL_GUN;

        base.Init();

        enemyKilledBySpecialGun = 0;

        EventDispatcher.Instance.RegisterListener(EventID.KillEnemyBySpecialGun, (sender, param) =>
        {
            enemyKilledBySpecialGun++;
        });
    }

    public override bool IsCompleted()
    {
        isCompleted = enemyKilledBySpecialGun >= requirement;

        return isCompleted;
    }

    public override string GetDescription()
    {
        string s = string.Format(description, requirement);

        return s;
    }

    public override string GetCurrentProgress()
    {
        string s = string.Format("{0}/{1}", enemyKilledBySpecialGun, requirement);

        return s;
    }
}
