using UnityEngine;
using System.Collections;
using System;

public class LocationBombardier : BaseSpawnLocation
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

            enemy.Rigid.bodyType = RigidbodyType2D.Kinematic;
            ((EnemyBomber)enemy).isFromLeft = false;
            ((EnemyBomber)enemy).Active(id, level, spawnPoint.position);

            GameController.Instance.AddUnit(enemy.gameObject, enemy);
            countSpawn++;

            yield return delay;
        }

        isSpawning = false;
        Clear();
        coroutineSpawn = null;
    }
}
