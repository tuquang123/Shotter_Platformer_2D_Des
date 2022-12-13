using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Map : MonoBehaviour
{
    public string stageNameId;
    public ControllerType controllerType;
    public Zone[] mainZones;

    //[Header("QUEST")]
    //public QuestController quest;

    [Header("PLAYER")]
    public bool isRamboStartOnJet;
    public Transform jetStartPoint;
    public Transform jetDestination;
    public Transform playerSpawnPoint;


    [Header("ENEMY")]
    public bool isAutoSpawnEnemy = true;
    public float timeAutoSpawn = 5f;
    public int minLevelAutoSpawn = 1;
    public int maxLevelAutoSpawn = 3;
    public int enemyPerSpawn = 1;
    public BaseEnemy[] enemyPrefabs;
    public BaseEnemy[] enemyAutoSpawnPrefabs;

    private int previousZoneId = 1;
    private int remainingMainUnit; // Remaining enemy must to be killed to pass to new lock point
    private Dictionary<int, int> mainEnemies = new Dictionary<int, int>(); // key = lockPointIndex, value = numberOfEnemy
    private List<int> passedZones = new List<int>();


    [Header("MAP DATA")]
    public MapData mapData;

    [Header("MARGIN")]
    public Transform marginLeft;
    public Transform marginTop;
    public Transform marginRight;
    public Transform marginBottom;
    public Transform mapEndPoint;
    public Transform cameraInitialPoint;

    public int CurrentZoneId { get; set; }
    public int CoinCompleteStage { get; set; }


    public void Init()
    {
        SetDefaultMapMargin();
        LoadMapData();
        SetCoinDrop();
        CreateTriggerPoints();
        InitFirstZone();

        //if (quest != null)
        //    quest.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, OnUnitDie);
        EventDispatcher.Instance.RegisterListener(EventID.EnterZone, OnEnterZone);
    }

    public void LockCurrentZone()
    {
        for (int i = 0; i < mainZones.Length; i++)
        {
            Zone zone = mainZones[i];

            if (zone.id == CurrentZoneId)
            {
                if (remainingMainUnit <= 0)
                {
                    SetDefaultMapMargin();
                    //Zone currentZone = GetCurrentZone();
                    zone.wallEnd.gameObject.SetActive(false);
                }
                else
                {
                    zone.Lock();
                }

                return;
            }
        }

        DebugCustom.Log("No main zone id matching to active");
    }

    public void SetDefaultMapMargin()
    {
        CameraFollow.Instance.SetMarginTop(marginTop.position.y);
        CameraFollow.Instance.SetMarginLeft(marginLeft.position.x);
        CameraFollow.Instance.SetMarginRight(marginRight.position.x);
        CameraFollow.Instance.SetMarginBottom(marginBottom.position.y);
    }

    public void AddMainUnitToCurrentZone(BaseUnit unit)
    {
        if (mainEnemies.ContainsKey(CurrentZoneId))
        {
            mainEnemies[CurrentZoneId]++;
        }
        else
        {
            mainEnemies.Add(CurrentZoneId, 1);
        }

        remainingMainUnit++;
    }

    public Zone GetCurrentZone()
    {
        int index = mainZones.Length;

        for (int i = 0; i < mainZones.Length; i++)
        {
            if (mainZones[i].id == CurrentZoneId)
            {
                return mainZones[i];
            }
        }

        return null;
    }

    private int GetZoneIndex(int zoneId)
    {
        int index = mainZones.Length;

        for (int i = 0; i < mainZones.Length; i++)
        {
            if (mainZones[i].id == zoneId)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    private void InitFirstZone()
    {
        CurrentZoneId = 1;
        CalculateMainUnits();
        LockCurrentZone();
    }

    private void CreateTriggerPoints()
    {
        TriggerPointBomber bomberPointPrefab = Resources.Load<TriggerPointBomber>(StaticValue.PATH_TRIGGER_POINT_BOMBER_PREFAB);
        TriggerPointHelicopter helicopterPointPrefab = Resources.Load<TriggerPointHelicopter>(StaticValue.PATH_TRIGGER_POINT_HELICOPTER_PREFAB);

        // Create bomber points
        if (mapData.bomberData != null)
        {
            GameObject bomberGroup = new GameObject("Bomber Trigger Points");
            bomberGroup.transform.parent = transform;

            for (int i = 0; i < mapData.bomberData.Count; i++)
            {
                BomberPointData data = mapData.bomberData[i];

                TriggerPointBomber point = Instantiate(bomberPointPrefab, bomberGroup.transform);
                point.transform.position = data.position;
                point.levelInNormal = data.levelInNormal;
                point.isFromLeft = data.isFromLeft;
            }
        }

        // Create helicopter points
        if (mapData.helicopterData != null)
        {
            GameObject helicopterGroup = new GameObject("Helicopter Trigger Points");
            helicopterGroup.transform.parent = transform;

            for (int i = 0; i < mapData.helicopterData.Count; i++)
            {
                HelicopterPointData data = mapData.helicopterData[i];

                TriggerPointHelicopter point = Instantiate(helicopterPointPrefab, helicopterGroup.transform);
                point.transform.position = data.position;

                point.levelInNormal = data.levelInNormal;
                point.isFinalBoss = data.isFinalBoss;
            }
        }
    }

    private void CalculateMainUnits()
    {
        if (mainEnemies.ContainsKey(CurrentZoneId))
        {
            remainingMainUnit = mainEnemies[CurrentZoneId];
            DebugCustom.Log(string.Format("Zone {0} has {1} main enemy", CurrentZoneId, remainingMainUnit));
        }
        else
        {
            remainingMainUnit = 0;
            DebugCustom.Log(string.Format("Zone {0} has no main enemy", CurrentZoneId));
        }
    }

    //private void CloneMapData(MapData data)
    //{
    //    mapData = new MapData();

    //    if (data.enemyData != null)
    //    {
    //        mapData.enemyData = new List<EnemySpawnData>();

    //        for (int i = 0; i < data.enemyData.Count; i++)
    //        {
    //            EnemySpawnData _data = data.enemyData[i];
    //            EnemySpawnData tmp = new EnemySpawnData(_data.id, _data.level, _data.position, _data.zoneId, _data.packId, _data.isMainUnit, _data.isCanMove, _data.isCanJump, _data.isRunPassArea, _data.items);
    //            tmp.index = i;
    //            mapData.enemyData.Add(tmp);
    //        }
    //    }

    //    if (data.bomberData != null)
    //    {
    //        mapData.bomberData = new List<BomberPointData>();

    //        for (int i = 0; i < data.bomberData.Count; i++)
    //        {
    //            BomberPointData _data = data.bomberData[i];
    //            BomberPointData tmp = new BomberPointData(_data.position, _data.isFromLeft, _data.levelInNormal);
    //            mapData.bomberData.Add(tmp);
    //        }
    //    }

    //    if (data.helicopterData != null)
    //    {
    //        mapData.helicopterData = new List<HelicopterPointData>();

    //        for (int i = 0; i < data.helicopterData.Count; i++)
    //        {
    //            HelicopterPointData _data = data.helicopterData[i];
    //            HelicopterPointData tmp = new HelicopterPointData(_data.position, _data.levelInNormal, _data.isFinalBoss);
    //            mapData.helicopterData.Add(tmp);
    //        }
    //    }

    //    if (data.bossData != null)
    //    {
    //        BossPointData tmp = new BossPointData(data.bossData.position, data.bossData.bossId);
    //        mapData.bossData = tmp;
    //    }
    //}

    private void LoadMapData()
    {
        mapData = MapUtils.GetMapData(stageNameId);
        //CloneMapData(tmp);

        if (mapData.enemyData == null)
            return;

        //SetEnemyDataByDifficulty();

        // Fill enemy data to dictionary
        for (int i = 0; i < mapData.enemyData.Count; i++)
        {
            EnemySpawnData eData = mapData.enemyData[i];

            if (eData.isMainUnit)
            {
                if (mainEnemies.ContainsKey(eData.zoneId))
                {
                    mainEnemies[eData.zoneId]++;
                }
                else
                {
                    mainEnemies.Add(eData.zoneId, 1);
                }
            }
        }
    }

    //private void SetEnemyDataByDifficulty()
    //{
    //    if (GameData.currentStage.difficulty == Difficulty.Normal)
    //        return;

    //    for (int i = 0; i < mapData.enemyData.Count; i++)
    //    {
    //        EnemySpawnData eData = mapData.enemyData[i];

    //        switch (GameData.currentStage.difficulty)
    //        {
    //            case Difficulty.Hard:
    //                eData.level += StaticValue.LEVEL_INCREASE_MODE_HARD;
    //                break;

    //            case Difficulty.Crazy:
    //                eData.level += StaticValue.LEVEL_INCREASE_MODE_CRAZY;
    //                break;
    //        }

    //        eData.level = Mathf.Clamp(eData.level, 1, StaticValue.MAX_LEVEL_ENEMY);
    //    }
    //}

    private void SetCoinDrop()
    {
        CoinCompleteStage = GameData.staticCampaignStageData.GetCoinCompleteStage(GameData.currentStage.id, GameData.currentStage.difficulty);

        if (mapData.enemyData == null)
            return;

        int coinFromEnemy = GameData.staticCampaignStageData.GetCoinDrop(GameData.currentStage.id, GameData.currentStage.difficulty);
        int totalEnemy = mapData.enemyData.Count;
        int numberEnemyDropCoin = Mathf.RoundToInt(totalEnemy * 0.7f);
        int averageCoinNormalUnit = numberEnemyDropCoin != 0 ? Mathf.RoundToInt((float)coinFromEnemy / (float)numberEnemyDropCoin) : 0;
        //DebugCustom.Log(string.Format("Total={0}, EnemyCount={1}, AverageCoin={2}", coinFromEnemy, totalEnemy, averageCoinNormalUnit));

        List<int> enemyIndexes = new List<int>();
        for (int i = 0; i < totalEnemy; i++)
        {
            enemyIndexes.Add(mapData.enemyData[i].index);
        }

        for (int i = 0; i < totalEnemy; i++)
        {
            EnemySpawnData enemyData = mapData.enemyData[i];

            if (enemyData.isMainUnit)
            {
                int bounty = Mathf.RoundToInt(averageCoinNormalUnit * 1f);

                if (bounty <= 0)
                {
                    bounty = 1;
                }

                enemyData.bounty = bounty;
                //DebugCustom.Log(string.Format("Index={0}, Bounty={1}", enemyData.index, enemyData.bounty));
                numberEnemyDropCoin--;
                enemyIndexes.Remove(enemyData.index);
            }
        }

        while (numberEnemyDropCoin > 0)
        {
            int random = Random.Range(0, enemyIndexes.Count);
            int bounty = averageCoinNormalUnit;
            //int bounty = Random.Range(averageCoinNormalUnit - 1, averageCoinNormalUnit + 2);

            if (bounty <= 0)
            {
                bounty = 1;
            }

            mapData.enemyData[enemyIndexes[random]].bounty = bounty;
            //DebugCustom.Log(string.Format("Index={0}, Bounty={1}", mapData.enemyData[enemyIndexes[random]].index, mapData.enemyData[enemyIndexes[random]].bounty));
            enemyIndexes.Remove(enemyIndexes[random]);
            numberEnemyDropCoin--;
        }

        //for (int i = 0; i < mapData.enemyData.Count; i++)
        //{
        //    DebugCustom.Log(string.Format("Index={0}, Bounty={1}", mapData.enemyData[i].index, mapData.enemyData[i].bounty));
        //}
    }

    private void OnEnterZone(Component senser, object param)
    {
        int zoneId = (int)param;

        previousZoneId = CurrentZoneId;
        CurrentZoneId = zoneId;
        CalculateMainUnits();

        if (remainingMainUnit <= 0)
        {
            OnCurrentZoneClear();
        }

        passedZones.Add(previousZoneId);
        ClearUnitPreviousZones();
    }

    private void OnUnitDie(Component senser, object param)
    {
        if (mainZones.Length <= 0)
            return;

        UnitDieData data = (UnitDieData)param;
        BaseEnemy enemy = data.unit.GetComponent<BaseEnemy>();

        if (enemy != null && enemy.isMainUnit)
        {
            if (mainEnemies.ContainsKey(enemy.zoneId))
            {
                mainEnemies[enemy.zoneId]--;
            }

            if (enemy.zoneId == CurrentZoneId)
            {
                remainingMainUnit--;
                DebugCustom.Log(string.Format("Current zone {0} remaining {1} main unit", CurrentZoneId, remainingMainUnit));

                if (remainingMainUnit <= 0)
                {
                    OnCurrentZoneClear(enemy);
                }
            }
        }
    }

    public void OnCurrentZoneClear(BaseUnit lastEnemy = null)
    {
        remainingMainUnit = 0;
        DebugCustom.Log(string.Format("Main zone {0} clear, GO !", CurrentZoneId));

        Zone currentZone = GetCurrentZone();
        currentZone.ShowObjects(true);

        if (currentZone.isLockWallEndWhenClear == false)
        {
            SetDefaultMapMargin();
        }

        if (currentZone.isFinalZone)
        {
            int previousZoneIndex = GetZoneIndex(previousZoneId);
            mainZones[previousZoneIndex].wallEnd.gameObject.SetActive(false);

            currentZone.Lock();

            UIController.Instance.ActiveIngameUI(false);

            if (lastEnemy != null)
            {
                CameraFollow.Instance.slowMotion.Show(callback: () =>
                 {
                     EventDispatcher.Instance.PostEvent(EventID.FinishStage, 0.5f);
                 });
            }
            else
            {
                EventDispatcher.Instance.PostEvent(EventID.FinishStage, 0.5f);
            }
        }
        else if (currentZone.isLockWallEndWhenClear == false)
        {
            currentZone.wallEnd.gameObject.SetActive(false);
            EventDispatcher.Instance.PostEvent(EventID.MoveCameraToNewZone, CurrentZoneId);

            if (currentZone.wallEndLockDir == CameraLockDirection.Right)
            {
                UIController.Instance.ShowArrowGo(true);
            }
            else if (currentZone.wallEndLockDir == CameraLockDirection.Left)
            {
                UIController.Instance.ShowArrowGo(false);
            }
        }
    }

    private void ClearUnitPreviousZones()
    {
        List<BaseUnit> deactiveEnemies = new List<BaseUnit>();

        foreach (BaseUnit unit in GameController.Instance.activeUnits.Values)
        {
            if (unit is BaseEnemy)
            {
                if (passedZones.Contains(((BaseEnemy)unit).zoneId) || ((BaseEnemy)unit).zoneId == -1)
                {
                    if (unit.IsOutOfScreen())
                        deactiveEnemies.Add(unit);
                }
            }
        }

        for (int i = 0; i < deactiveEnemies.Count; i++)
        {
            deactiveEnemies[i].Deactive();
        }
    }
}
