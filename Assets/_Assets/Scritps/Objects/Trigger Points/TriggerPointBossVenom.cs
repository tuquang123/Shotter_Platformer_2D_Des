using UnityEngine;
using System.Collections;

public class TriggerPointBossVenom : TriggerPointBoss
{
    public Transform furthestLaserPoint;
    public Transform nearestLaserPoint;


    protected override void SpawnBoss()
    {
        if (gameObject.activeInHierarchy)
        {
            BossVenom boss = Instantiate(bossPrefab) as BossVenom;

            boss.isInvisibleWhenActive = true;
            boss.FurthestLaserPoint = furthestLaserPoint;
            boss.NearestLaserPoint = nearestLaserPoint;

            int level = GetLevel();
            boss.Active(bossPrefab.id, level, spawnPosition.position);
            boss.SetTarget(GameController.Instance.Player);

            GameController.Instance.AddUnit(boss.gameObject, boss);
            UIController.Instance.hudBoss.UpdateHP(1f);
            SwitchMusic();
        }
    }
}
