using UnityEngine;
using System.Collections;

public class TriggerPointBossMonkey : TriggerPointBoss
{
    public Transform minionSpawnPoint;
    public Transform minionMostLeftPoint;
    public Transform minionMostRightPoint;


    protected override void SpawnBoss()
    {
        if (gameObject.activeInHierarchy)
        {
            BossMonkey boss = Instantiate(bossPrefab) as BossMonkey;

            boss.basePosition = basePosition.position;
            boss.SetPoints(minionSpawnPoint.position, minionMostLeftPoint.position, minionMostRightPoint.position);
            int level = GetLevel();
            boss.Active(bossPrefab.id, level, spawnPosition.position);
            boss.SetTarget(GameController.Instance.Player);

            GameController.Instance.AddUnit(boss.gameObject, boss);
            UIController.Instance.hudBoss.UpdateHP(1f);
            SwitchMusic();
        }
    }
}
