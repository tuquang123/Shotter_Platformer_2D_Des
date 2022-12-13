using UnityEngine;
using System.Collections;

public class LocationHelicopter : BaseSpawnLocation
{
    private WaitForSeconds delay = new WaitForSeconds(2f);

    public override void Spawn()
    {
        if (coroutineSpawn == null)
        {
            coroutineSpawn = CoroutineSpawn();
            StartCoroutine(coroutineSpawn);
        }
    }

    private IEnumerator CoroutineSpawn()
    {
        isSpawning = true;
        int countSpawn = 0;

        while (countSpawn < spawnUnits.Count)
        {
            int id = (int)spawnUnits[countSpawn];
            int level = UnityEngine.Random.Range(minLevelUnit, maxLevelUnit + 1);
            BaseEnemy enemyPrefab = GameController.Instance.modeController.GetEnemyPrefab((int)spawnUnits[countSpawn]);
            BaseEnemy enemy = enemyPrefab.GetFromPool();
            EnemyHelicopter helicopter = (EnemyHelicopter)enemy;

            Vector2 position = CameraFollow.Instance.pointAirSpawnRight.position;

            helicopter.Active(id, level, position);
            helicopter.GetNextDestination();
            helicopter.SetTarget(GameController.Instance.Player);

            GameController.Instance.AddUnit(enemy.gameObject, enemy);
            ((SurvivalModeController)GameController.Instance.modeController).AddUnit(enemy);
            countSpawn++;

            yield return delay;
        }

        isSpawning = false;
        Clear();
        coroutineSpawn = null;
    }
}
