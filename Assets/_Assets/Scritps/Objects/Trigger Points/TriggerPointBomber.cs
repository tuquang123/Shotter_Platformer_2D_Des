using UnityEngine;
using System.Collections;

public class TriggerPointBomber : MonoBehaviour
{
    public EnemyBomber bomberPrefab;
    public int levelInNormal = 1;
    public bool isFromLeft;

    private BoxCollider2D sensor;


    void Awake()
    {
        sensor = GetComponent<BoxCollider2D>();

        if (sensor != null)
        {
            sensor.enabled = true;
        }
        else
        {
            DebugCustom.LogError("Helicopter area sensor NULL");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            SpawnBombardier();
            sensor.enabled = false;
        }
    }

    private void SpawnBombardier()
    {
        BaseEnemy enemy = bomberPrefab.GetFromPool();

        Vector2 position = isFromLeft ? CameraFollow.Instance.pointAirSpawnLeft.position : CameraFollow.Instance.pointAirSpawnRight.position;
        int level = levelInNormal;

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

        ((EnemyBomber)enemy).isFromLeft = isFromLeft;
        ((EnemyBomber)enemy).Active(bomberPrefab.id, level, position);

        GameController.Instance.AddUnit(enemy.gameObject, enemy);
    }
}
