using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HouseSpawnEnemy : MonoBehaviour
{
    public bool isFinalHouse;
    public Transform lockPointTop;
    public Transform spawnPoint;
    public Transform mostLeftPoint;
    public Transform mostRightPoint;
    public GameObject door;
    public int totalUnits = 5;
    public int levelUnit = 3;
    public float timeDelaySpawn = 1f;
    public BaseEnemy[] enemyPrefabs;

    private bool isActive;
    private int remainingUnits;
    private int bountyPerUnit;
    private WaitForSeconds waitSpawn;
    private Dictionary<GameObject, BaseUnit> activeUnits = new Dictionary<GameObject, BaseUnit>();

    private void Awake()
    {
        waitSpawn = new WaitForSeconds(timeDelaySpawn);
    }

    private void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, OnUnitDie);

        if (GameData.mode == GameMode.Campaign)
        {
            int coin = GameData.staticCampaignStageData.GetCoinDrop(GameData.currentStage.id, GameData.currentStage.difficulty);
            bountyPerUnit = Mathf.RoundToInt((float)coin * 0.1f / (float)totalUnits);
        }
    }

    public void Open()
    {
        CameraFollow.Instance.SetMarginTop(lockPointTop.position.y);

        float y = door.transform.position.y;
        y += 1.4f;

        door.transform.DOMoveY(y, 1.5f).OnComplete(() =>
        {
            isActive = true;
            StartCoroutine(CoroutineSpawnUnits());
        });
    }

    private void OnUnitDie(Component senser, object param)
    {
        if (isActive == false)
            return;

        UnitDieData data = (UnitDieData)param;
        BaseEnemy enemy = data.unit.GetComponent<BaseEnemy>();

        if (activeUnits.ContainsKey(enemy.gameObject))
        {
            remainingUnits--;
            activeUnits.Remove(enemy.gameObject);
        }

        if (remainingUnits <= 0)
        {
            float marginTop = GameController.Instance.CampaignMap.marginTop.position.y;
            CameraFollow.Instance.SetMarginTop(marginTop);
            isActive = false;

            if (isFinalHouse)
            {
                CameraFollow.Instance.slowMotion.Show(callback: () =>
                {
                    EventDispatcher.Instance.PostEvent(EventID.FinishStage, 0.5f);
                });
            }
        }
    }

    private IEnumerator CoroutineSpawnUnits()
    {
        remainingUnits = totalUnits;
        int count = 0;

        int level = levelUnit;

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

        while (count < totalUnits)
        {
            BaseEnemy enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            BaseEnemy enemy = enemyPrefab.GetFromPool();
            enemy.isInvisibleWhenActive = true;
            enemy.Active(enemyPrefab.id, level, spawnPoint.position);
            enemy.zoneId = -1;
            enemy.canMove = false;
            enemy.ActiveSensor(false);
            enemy.bounty = bountyPerUnit;

            Vector2 v = mostLeftPoint.position;
            v.x = Random.Range(mostLeftPoint.position.x, mostRightPoint.position.x);
            float s = Vector2.Distance(transform.position, v);

            enemy.transform.DOMove(v, s / enemy.baseStats.MoveSpeed).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (enemy.isDead == false)
                {
                    enemy.isImmortal = false;
                    enemy.Rigid.simulated = true;
                    enemy.PlayAnimationIdle();
                    enemy.ActiveSensor(true);
                }

            }).OnStart(() =>
            {
                enemy.isImmortal = true;
                enemy.Rigid.simulated = false;
                enemy.skeletonAnimation.Skeleton.flipX = (v.x < enemy.transform.position.x);
                enemy.PlayAnimationMove();
            });

            GameController.Instance.AddUnit(enemy.gameObject, enemy);
            activeUnits.Add(enemy.gameObject, enemy);

            count++;
            yield return waitSpawn;
        }
    }
}
