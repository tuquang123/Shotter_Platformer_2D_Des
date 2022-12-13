using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LabDoorSpawnEnemy : MonoBehaviour
{
    public int totalUnits = 5;
    public int minLevel = 1;
    public int maxLevel = 3;
    public float timeBetweenSpawn = 3f;
    public float doorMoveSpeed = 2f;
    public Transform[] spawnPoints;
    public BaseEnemy[] enemyPrefabs;

    private CircleCollider2D sensor;
    private AudioClip soundAlarm;
    private WaitForSeconds delayAlarm;
    private Vector2 destination;
    private string methodNameSpawn = "SpawnUnit";
    private int unitSpawned;
    private int bountyPerUnit;
    private bool isAlarm;
    private bool isOpeningDoor;
    private Dictionary<GameObject, BaseUnit> activeUnits = new Dictionary<GameObject, BaseUnit>();


    private void Awake()
    {
        sensor = GetComponent<CircleCollider2D>();

        Vector2 v = transform.position;
        v.y += 1.36f;
        destination = v;
    }

    private void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, OnUnitDie);

        InitAlarm();

        if (GameData.mode == GameMode.Campaign)
        {
            int coin = GameData.staticCampaignStageData.GetCoinDrop(GameData.currentStage.id, GameData.currentStage.difficulty);
            bountyPerUnit = Mathf.RoundToInt((float)coin * 0.1f / (float)totalUnits);
        }
    }

    private void Update()
    {
        if (isOpeningDoor)
        {
            if (Mathf.Abs(destination.y - transform.position.y) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, destination, doorMoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = destination;
                isOpeningDoor = false;
                SpawnUnit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            OnAlarm();
        }
    }

    public void OnAlarm()
    {
        isOpeningDoor = true;
        isAlarm = true;
        StartCoroutine(CoroutineAlarm());
        sensor.enabled = false;

        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_DOOR_OPEN);
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
