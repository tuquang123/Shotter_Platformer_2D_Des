using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MilitaryBase : MonoBehaviour
{
    public Transform door;
    public Transform doorDestination;
    public GameObject halfTopDoor;
    public MilitaryBaseSensor sensor;
    public int totalUnits = 5;
    public int minLevel;
    public int maxLevel = 5;
    public float timeBetweenSpawn = 3f;
    public float doorMoveSpeed = 1f;
    public Transform[] spawnPoints;
    public BaseEnemy[] enemyPrefabs;

    private AudioClip soundAlarm;
    private WaitForSeconds delayAlarm;
    private Vector3 pointHideHalfDoor;
    private string methodNameSpawn = "SpawnUnit";
    private int unitSpawned;
    private int bountyPerUnit;
    private bool isAlarm;
    private bool isOpeningDoor;
    private Dictionary<GameObject, BaseUnit> activeUnits = new Dictionary<GameObject, BaseUnit>();

    void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, OnUnitDie);

        InitAlarm();

        if (GameData.mode == GameMode.Campaign)
        {
            int coin = GameData.staticCampaignStageData.GetCoinDrop(GameData.currentStage.id, GameData.currentStage.difficulty);
            bountyPerUnit = Mathf.RoundToInt((float)coin * 0.1f / (float)totalUnits);
        }
    }

    void Update()
    {
        if (isOpeningDoor)
        {
            if (Mathf.Abs(doorDestination.position.y - door.position.y) > 0.1f)
            {
                door.position = Vector2.MoveTowards(door.position, doorDestination.position, doorMoveSpeed * Time.deltaTime);

                if (halfTopDoor.activeInHierarchy && Mathf.Abs(pointHideHalfDoor.y - door.position.y) <= 0.05f)
                {
                    halfTopDoor.SetActive(false);
                }
            }
            else
            {
                door.position = doorDestination.position;
                isOpeningDoor = false;
                SpawnUnit();
            }
        }
    }

    public void OnAlarm()
    {
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_DOOR_OPEN);

        isOpeningDoor = true;
        isAlarm = true;
        StartCoroutine(CoroutineAlarm());
        sensor.gameObject.SetActive(false);
    }

    private void InitAlarm()
    {
        soundAlarm = SoundManager.Instance.GetAudioClip(StaticValue.SOUND_SFX_MILITARY_ALARM);

        if (soundAlarm != null)
        {
            delayAlarm = new WaitForSeconds(soundAlarm.length + 1f);
        }
        else
        {
            DebugCustom.LogError("Sound alarm NULL");
        }

        Vector3 v = doorDestination.position;
        v.y -= 0.76f;
        pointHideHalfDoor = v;
    }

    private void SpawnUnit()
    {
        if (unitSpawned >= totalUnits)
        {
            enabled = false;
            StopAllCoroutines();
            //UIController.Instance.alarmRedScreen.SetActive(false);

            return;
        }

        Vector2 position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;


        if (GameData.mode == GameMode.Campaign)
        {
            //int level = Random.Range(minLevel, maxLevel + 1);

            //if (GameData.currentStage.difficulty == Difficulty.Hard)
            //{
            //    level += StaticValue.LEVEL_INCREASE_MODE_HARD;
            //}
            //else if (GameData.currentStage.difficulty == Difficulty.Crazy)
            //{
            //    level += StaticValue.LEVEL_INCREASE_MODE_CRAZY;
            //}

            //level = Mathf.Clamp(level, 1, StaticValue.MAX_LEVEL_ENEMY);

            int maxLevel = GameData.staticCampaignStageData.GetLevelEnemy(GameData.currentStage.id, GameData.currentStage.difficulty);
            int level = Random.Range(1, maxLevel + 1);

            BaseEnemy enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            BaseEnemy enemy = enemyPrefab.GetFromPool();
            enemy.isInvisibleWhenActive = true;
            enemy.Active(enemyPrefab.id, level, position);
            enemy.zoneId = -1;
            enemy.canMove = true;
            enemy.canJump = true;
            enemy.isRunPassArea = true;
            enemy.bounty = bountyPerUnit;

            GameController.Instance.AddUnit(enemy.gameObject, enemy);
            activeUnits.Add(enemy.gameObject, enemy);

            unitSpawned++;
            Invoke(methodNameSpawn, timeBetweenSpawn);
        }
    }

    private void OnUnitDie(Component senser, object param)
    {
        UnitDieData data = (UnitDieData)param;
        BaseEnemy enemy = data.unit.GetComponent<BaseEnemy>();

        if (activeUnits.ContainsKey(enemy.gameObject))
        {
            CancelInvoke();
            activeUnits.Remove(enemy.gameObject);
            Invoke(methodNameSpawn, 1f);
        }
    }

    private IEnumerator CoroutineAlarm()
    {
        SoundManager.Instance.PlaySfx(soundAlarm);
        //UIController.Instance.alarmRedScreen.SetActive(true);

        while (isAlarm)
        {
            yield return delayAlarm;
            SoundManager.Instance.PlaySfx(soundAlarm);
        }
    }
}
