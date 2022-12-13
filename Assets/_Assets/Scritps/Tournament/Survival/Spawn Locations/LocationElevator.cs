using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class LocationElevator : BaseSpawnLocation
{

    private Vector2 startPosition;
    private Vector2 endPosition;
    private List<BaseEnemy> units = new List<BaseEnemy>();


    private void Awake()
    {
        startPosition = transform.position;

        Vector2 v = startPosition;
        v.y -= 7.5f;
        endPosition = v;
    }

    public override void Spawn()
    {
        SpawnUnitOnElevator();
        MoveDown();
    }

    private void SpawnUnitOnElevator()
    {
        for (int i = 0; i < spawnUnits.Count; i++)
        {
            int id = (int)spawnUnits[i];
            int level = UnityEngine.Random.Range(minLevelUnit, maxLevelUnit + 1);
            BaseEnemy enemyPrefab = GameController.Instance.modeController.GetEnemyPrefab((int)spawnUnits[i]);
            BaseEnemy enemy = enemyPrefab.GetFromPool();

            Vector2 v = spawnPoint.position;
            v.x += UnityEngine.Random.Range(-1.5f, 1.5f);

            enemy.farSensor.col.radius = 30f;
            enemy.Active(id, level, v);
            enemy.transform.parent = transform;
            enemy.canMove = true;
            enemy.canJump = false;
            enemy.isRunPassArea = true;
            enemy.ActiveSensor(false);
            enemy.isImmortal = true;
            enemy.Rigid.simulated = false;
            enemy.skeletonAnimation.Skeleton.FlipX = UnityEngine.Random.Range(0, 2) == 0;
            enemy.PlayAnimationIdle();
            enemy.enabled = false;

            units.Add(enemy);
            GameController.Instance.AddUnit(enemy.gameObject, enemy);
            ((SurvivalModeController)GameController.Instance.modeController).AddUnit(enemy);
        }

        Clear();
    }

    private void MoveDown()
    {
        isSpawning = true;

        float y = endPosition.y;

        transform.DOMoveY(y, 3f).OnComplete(() =>
        {
            for (int i = 0; i < units.Count; i++)
            {
                BaseEnemy enemy = units[i];
                enemy.transform.parent = null;
                enemy.isImmortal = false;
                enemy.Rigid.simulated = true;
                enemy.ActiveSensor(true);
                enemy.enabled = true;
            }

            units.Clear();

            MoveUp(2f);
        });
    }

    private void MoveUp(float delay)
    {
        float y = startPosition.y;

        transform.DOMoveY(y, 2f).SetDelay(2f).OnComplete(() =>
        {
            isSpawning = false;
        });
    }
}
