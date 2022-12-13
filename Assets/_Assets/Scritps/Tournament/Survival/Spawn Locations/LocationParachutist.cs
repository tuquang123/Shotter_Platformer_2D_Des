using UnityEngine;
using System.Collections;
using System;

public class LocationParachutist : BaseSpawnLocation
{
    public Transform mostLeftPoint;
    public Transform mostRightPoint;

    public override void Spawn()
    {
        for (int i = 0; i < spawnUnits.Count; i++)
        {
            int id = (int)spawnUnits[i];
            int level = UnityEngine.Random.Range(minLevelUnit, maxLevelUnit + 1);
            BaseEnemy enemyPrefab = GameController.Instance.modeController.GetEnemyPrefab((int)spawnUnits[i]);
            BaseEnemy enemy = enemyPrefab.GetFromPool();

            Vector2 v = mostLeftPoint.position;
            v.x = UnityEngine.Random.Range(mostLeftPoint.position.x, mostRightPoint.position.x);

            enemy.farSensor.col.radius = 30f;
            enemy.Active(id, level, v);
            enemy.canMove = UnityEngine.Random.Range(1, 101) > 70;
            enemy.canJump = false;
            enemy.isRunPassArea = true;
            enemy.ActiveSensor(true);
            enemy.isImmortal = false;

            GameController.Instance.AddUnit(enemy.gameObject, enemy);
            ((SurvivalModeController)GameController.Instance.modeController).AddUnit(enemy);
        }

        Clear();
    }
}
