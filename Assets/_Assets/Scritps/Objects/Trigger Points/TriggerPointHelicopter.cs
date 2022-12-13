using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerPointHelicopter : MonoBehaviour
{
    public EnemyHelicopter helicopterPrefabs;
    public Collider2D wallStart;
    public Collider2D wallEnd;
    public int levelInNormal = 1;
    public bool isFinalBoss;

    private Collider2D sensor;
    private Dictionary<GameObject, EnemyHelicopter> activeUnits = new Dictionary<GameObject, EnemyHelicopter>();


    void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, OnUnitDie);

        sensor = GetComponent<BoxCollider2D>();

        if (sensor != null)
        {
            sensor.enabled = true;
        }
        else
        {
            DebugCustom.LogError("Helicopter area sensor NULL");
        }

        wallStart.gameObject.SetActive(false);
        wallEnd.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            SpawnHelicopter();
            LockArea(true);
            sensor.enabled = false;
        }
    }

    private void LockArea(bool isLock)
    {
        wallStart.gameObject.SetActive(isLock);
        wallEnd.gameObject.SetActive(isLock);

        if (isLock)
        {
            CameraFollow.Instance.SetMarginLeft(wallStart.transform.position.x);
            CameraFollow.Instance.SetMarginRight(wallEnd.transform.position.x);
        }
        else
        {
            //Zone zone = GameController.Instance.Map.GetCurrentZone();
            //zone.SetCameraMargin();

            CameraFollow.Instance.SetMarginRight(wallEnd.transform.position.x);
        }
    }

    private void SpawnHelicopter()
    {
        BaseEnemy enemy = helicopterPrefabs.GetFromPool();
        EnemyHelicopter helicopter = (EnemyHelicopter)enemy;

        Vector2 position = CameraFollow.Instance.pointAirSpawnRight.position;
        int level = levelInNormal;

        if (GameData.mode == GameMode.Campaign)
        {
            if (GameData.currentStage.difficulty == Difficulty.Hard)
            {
                level += StaticValue.LEVEL_INCREASE_MODE_HARD;
            }
            else if (GameData.currentStage.difficulty == Difficulty.Crazy)
            {
                level += StaticValue.LEVEL_INCREASE_MODE_CRAZY;
            }

            level = Mathf.Clamp(level, 1, StaticValue.MAX_LEVEL_ENEMY);
        }

        helicopter.Active(helicopterPrefabs.id, level, position);
        helicopter.GetNextDestination();
        helicopter.isMainUnit = true;
        helicopter.zoneId = GameController.Instance.CampaignMap.CurrentZoneId;
        helicopter.isMiniBoss = isFinalBoss;
        helicopter.SetTarget(GameController.Instance.Player);

        activeUnits.Add(helicopter.gameObject, helicopter);
        GameController.Instance.AddUnit(helicopter.gameObject, helicopter);
        GameController.Instance.CampaignMap.AddMainUnitToCurrentZone(helicopter);
    }

    private void OnUnitDie(Component senser, object param)
    {
        UnitDieData data = (UnitDieData)param;
        BaseEnemy enemy = data.unit.GetComponent<BaseEnemy>();

        if (activeUnits.ContainsKey(enemy.gameObject))
        {
            CancelInvoke();
            activeUnits.Remove(enemy.gameObject);

            if (activeUnits.Count <= 0)
            {
                LockArea(false);
                GameController.Instance.CampaignMap.SetDefaultMapMargin();
                //CameraFollow.Instance.SetMarginLeft(wallStart.transform.position.x);
            }
        }
    }
}
