using UnityEngine;
using System.Collections;

public class QuestKillFinalBossInTime : BaseQuest
{
    public float timeRequirement;

    private float timeStartBoss;
    private float timeBossDie;
    private int bossId;

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_KILL_FINAL_BOSS_IN_TIME;

        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.FinalBossStart, (sender, param) =>
        {
            timeStartBoss = Time.time;
        });

        EventDispatcher.Instance.RegisterListener(EventID.FinalBossDie, (sender, param) =>
        {
            timeBossDie = Time.time;
            bossId = (int)param;
        });
    }

    public override bool IsCompleted()
    {
        float timeKillBoss = timeBossDie - timeStartBoss;
        isCompleted = timeKillBoss <= timeRequirement;

        if (isCompleted)
        {
            FirebaseAnalyticsHelper.LogEvent("N_KillBossTime",
            "BossID=" + bossId,
            "Time=" + timeKillBoss);
        }

        return isCompleted;
    }

    public override string GetDescription()
    {
        string s = string.Format(description, timeRequirement);

        return s;
    }
}
