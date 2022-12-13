using UnityEngine;
using System.Collections;
using DG.Tweening;

public class LocationHouse : BaseSpawnLocation
{
    public GameObject door;

    private Vector2 doorClosePosition;


    private void Awake()
    {
        doorClosePosition = door.transform.position;
    }

    public override void Spawn()
    {
        OpenDoor();
    }

    private IEnumerator CorountineSpawn()
    {
        int countSpawn = 0;

        while (countSpawn < spawnUnits.Count)
        {
            int id = (int)spawnUnits[countSpawn];
            int level = Random.Range(minLevelUnit, maxLevelUnit + 1);
            BaseEnemy enemyPrefab = GameController.Instance.modeController.GetEnemyPrefab((int)spawnUnits[countSpawn]);
            BaseEnemy enemy = enemyPrefab.GetFromPool();

            enemy.isInvisibleWhenActive = true;

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
            enemy.isRunPassArea = Random.Range(1, 11) <= 5;
            enemy.ActiveSensor(false);

            Vector2 v = spawnPoint.position;
            v.x += Random.Range(-2f, 2f);
            float s = Vector2.Distance(transform.position, v);

            enemy.transform.DOMove(v, s / enemy.baseStats.MoveSpeed).SetEase(Ease.Linear).OnComplete(() =>
            {
                //enemy.isImmortal = false;
                enemy.Rigid.simulated = true;
                enemy.PlayAnimationIdle();
                enemy.ActiveSensor(true);
                enemy.enabled = true;

            }).OnStart(() =>
            {
                //enemy.isImmortal = true;
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

        Clear();
        CloseDoor();
        coroutineSpawn = null;
    }

    private void OpenDoor()
    {
        isSpawning = true;

        float y = door.transform.position.y;
        y += 1.4f;

        door.transform.DOMoveY(y, 1.5f).OnComplete(() =>
        {
            if (coroutineSpawn == null)
            {
                coroutineSpawn = CorountineSpawn();
                StartCoroutine(coroutineSpawn);
            }
        });
    }

    private void CloseDoor()
    {
        door.transform.DOMove(doorClosePosition, 1.5f).OnComplete(() =>
        {
            isSpawning = false;
        });
    }
}
