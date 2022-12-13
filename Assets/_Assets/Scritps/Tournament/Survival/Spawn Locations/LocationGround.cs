using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

public class LocationGround : BaseSpawnLocation
{
    public Transform startPosition;

    public override void Spawn()
    {
        if (coroutineSpawn == null)
        {
            coroutineSpawn = CorountineSpawn();
            StartCoroutine(coroutineSpawn);
        }
    }

    private IEnumerator CorountineSpawn()
    {
        isSpawning = true;
        int countSpawn = 0;

        while (countSpawn < spawnUnits.Count)
        {
            int id = (int)spawnUnits[countSpawn];
            int level = UnityEngine.Random.Range(minLevelUnit, maxLevelUnit + 1);
            //DebugCustom.Log(string.Format("LocationId={0}, LocationName={2}, SpawnUnit={1}, Level={3}", id, spawnUnits[countSpawn], name, level));
            BaseEnemy enemyPrefab = GameController.Instance.modeController.GetEnemyPrefab((int)spawnUnits[countSpawn]);
            BaseEnemy enemy = enemyPrefab.GetFromPool();

            enemy.farSensor.col.radius = 30f;
            enemy.Active(id, level, spawnPoint.position);

            if (enemy is EnemyKnife || enemy is EnemyMonkey || enemy is EnemyFire)
            {
                enemy.canMove = true;
            }
            else
            {
                enemy.canMove = UnityEngine.Random.Range(1, 101) > 70;
            }

            enemy.canJump = false;
            enemy.isRunPassArea = UnityEngine.Random.Range(1, 11) <= 5;
            enemy.ActiveSensor(false);

            Vector2 v = startPosition.position;
            v.x += UnityEngine.Random.Range(-2f, 2f);
            float s = Vector2.Distance(transform.position, v);

            enemy.transform.DOMove(v, s / enemy.baseStats.MoveSpeed).SetEase(Ease.Linear).OnComplete(() =>
            {
                enemy.isImmortal = false;
                enemy.Rigid.simulated = true;
                enemy.PlayAnimationIdle();
                enemy.ActiveSensor(true);
                enemy.enabled = true;

            }).OnStart(() =>
            {
                enemy.isImmortal = true;
                enemy.Rigid.simulated = false;
                enemy.SetDestinationMove(v);
                enemy.skeletonAnimation.Skeleton.flipX = (v.x < transform.position.x);
                enemy.PlayAnimationMove();
                enemy.enabled = false;
            });

            GameController.Instance.AddUnit(enemy.gameObject, enemy);
            ((SurvivalModeController)GameController.Instance.modeController).AddUnit(enemy);
            countSpawn++;

            yield return delaySpawn;
        }

        isSpawning = false;
        Clear();
        coroutineSpawn = null;
    }
}
