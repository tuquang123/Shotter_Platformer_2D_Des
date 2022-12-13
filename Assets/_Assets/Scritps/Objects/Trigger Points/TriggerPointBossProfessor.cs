using UnityEngine;
using System.Collections;

public class TriggerPointBossProfessor : TriggerPointBoss
{
    public Transform minionSpawnLeftPoint;
    public Transform minionSpawnRightPoint;


    protected override void SpawnBoss()
    {
        if (gameObject.activeInHierarchy)
        {
            BossProfessor boss = Instantiate(bossPrefab) as BossProfessor;

            boss.basePosition = basePosition.position;
            boss.SetPointSpawnMinion(minionSpawnLeftPoint.position, minionSpawnRightPoint.position);
            int level = GetLevel();
            boss.Active(bossPrefab.id, level, spawnPosition.position);
            boss.SetTarget(GameController.Instance.Player);

            GameController.Instance.AddUnit(boss.gameObject, boss);
            UIController.Instance.hudBoss.UpdateHP(1f);
            SwitchMusic();
        }
    }
}
