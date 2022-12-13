using UnityEngine;
using System.Collections;

public class TriggerPointSpawnWaveEnemy : MonoBehaviour
{
    public int totalEnemies = 10;
    public int minLevel = 1;
    public int maxLevel = 3;
    public Transform spawnPointCenter;
    public BaseEnemy[] enemyPrefabs;

    private BoxCollider2D col;


    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            Spawn();
            col.enabled = false;
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            int randomEnemyId = Random.Range(0, enemyPrefabs.Length);
            int id = enemyPrefabs[randomEnemyId].id;
            int level = Random.Range(minLevel, maxLevel + 1);

            if (GameData.mode == GameMode.Campaign)
            {
                level = GameData.staticCampaignStageData.GetLevelEnemy(GameData.currentStage.id, GameData.currentStage.difficulty);

                //if (GameData.currentStage.difficulty == Difficulty.Hard)
                //{
                //    level += StaticValue.LEVEL_INCREASE_MODE_HARD;
                //}
                //else if (GameData.currentStage.difficulty == Difficulty.Crazy)
                //{
                //    level += StaticValue.LEVEL_INCREASE_MODE_CRAZY;
                //}

                //level = Mathf.Clamp(level, 1, StaticValue.MAX_LEVEL_ENEMY);
            }

            BaseEnemy enemyPrefab = GetEnemyPrefab(id);
            BaseEnemy enemy = enemyPrefab.GetFromPool();

            Vector2 v = spawnPointCenter.position;
            v.x += Random.Range(-5f, 5f);

            enemy.Active(id, level, v);
            enemy.SetTarget(GameController.Instance.Player);

            GameController.Instance.AddUnit(enemy.gameObject, enemy);
        }
    }

    private BaseEnemy GetEnemyPrefab(int id)
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            BaseEnemy enemyPrefab = enemyPrefabs[i];

            if (enemyPrefab.id == id)
            {
                return enemyPrefab;
            }
        }

        return null;
    }
}
