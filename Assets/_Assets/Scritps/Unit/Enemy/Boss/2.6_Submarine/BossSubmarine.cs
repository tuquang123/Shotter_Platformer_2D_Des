using UnityEngine;
using System.Collections;
using Spine.Unity;
using DG.Tweening;
using Spine;
using System.Collections.Generic;

public enum BossMarineAction
{
    None,
    Rocket,
    Torpedo,
    SpawnMarine,
    Gore
}

public class BossSubmarine : BaseEnemy
{
    [Header("BOSS SUBMARINE PROPERTIES")]
    public RocketBossSubmarine rocketPrefab;
    public BaseMuzzle dustMuzzlePrefab;
    public Torpedo torpedoPrefab;
    public EnemyMarine marinePrefab;
    public Transform rocketFirePoint;
    public Transform torpedoFirePoint;
    public Transform marineSpawnPoint;
    public BossSubmarineColliderGore colliderGore;
    [SpineAnimation]
    public string shootRocket, idleToRocket, spawn, shootTorpedo;
    [SpineEvent]
    public string eventShootRocket, eventShootTorpedo, eventSink;
    public AudioClip soundOpenDoor, soundShootRocket, soundShootTorpedo;
    public AudioClip[] soundMarineAppear;

    private BaseMuzzle dustMuzzle;
    private bool isMovingToBase = true;
    private bool flagRocket;
    private bool flagTorpedo;
    private bool flagMarine;
    private bool isSpawningMarines;
    private float underWaterPosY;
    private Vector3 destinationRocket;
    private WaitForSeconds delayRocket;
    private WaitForSeconds delaySpawnMarine;
    private BossMarineAction nextAction;
    private Dictionary<GameObject, EnemyMarine> activeMarines = new Dictionary<GameObject, EnemyMarine>();

    protected override void Start()
    {
        base.Start();

        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, OnUnitDie);
        EventDispatcher.Instance.RegisterListener(EventID.BoatTriggerWater, OnRubberBoatTriggerWater);
    }

    protected override void Update()
    {
        if (isDead == false)
        {
            MoveToBase();

            if (isReadyAttack)
            {
                Attack();
            }
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_BOSS_SUBMARINE, level);
        baseStats = Resources.Load<SO_BossSubmarineStats>(path);
    }

    public override void Renew()
    {
        base.Renew();

        isFinalBoss = true;
        isEffectMeleeWeapon = false;
        bodyCollider.gameObject.SetActive(false);
        delayRocket = new WaitForSeconds(((SO_BossSubmarineStats)baseStats).TimeDelayRocket);
        delaySpawnMarine = new WaitForSeconds(((SO_BossSubmarineStats)baseStats).TimeDelaySpawnMarine);
    }

    public override void UpdateHealthBar(bool isAutoHide = false)
    {
        UIController.Instance.hudBoss.SetIconBoss(id);
        UIController.Instance.hudBoss.UpdateHP(HpPercent);
    }

    protected override void Attack()
    {
        if (flagRocket)
        {
            flagRocket = false;
            skeletonAnimation.AnimationState.SetAnimation(0, idleToRocket, false);
        }
        else if (flagTorpedo)
        {
            flagTorpedo = false;
            StartCoroutine(ReleaseTorpedo());
        }
        else if (flagMarine)
        {
            flagMarine = false;
            skeletonAnimation.AnimationState.SetAnimation(0, spawn, false);

            SoundManager.Instance.PlaySfx(soundOpenDoor);
        }
    }

    protected override void StartDie()
    {
        base.StartDie();

        EventDispatcher.Instance.PostEvent(EventID.BossSubmarineDie);
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, eventShootRocket) == 0)
        {
            RocketBossSubmarine rocket = PoolingController.Instance.poolRocketBossSubmarine.New();

            if (rocket == null)
            {
                rocket = Instantiate(rocketPrefab) as RocketBossSubmarine;
            }

            float damage = HpPercent > 0.5f ? ((SO_BossSubmarineStats)baseStats).RocketDamage : ((SO_BossSubmarineStats)baseStats).RageRocketDamage;
            float rocketSpeed = HpPercent > 0.5f ? ((SO_BossSubmarineStats)baseStats).RocketSpeed : ((SO_BossSubmarineStats)baseStats).RageRocketSpeed;

            AttackData atkData = new AttackData(this, damage);
            rocket.Active(atkData, rocketFirePoint, destinationRocket, rocketSpeed, PoolingController.Instance.groupBullet);

            ActiveDustMuzzle(rocketFirePoint);

            SoundManager.Instance.PlaySfx(soundShootRocket);
        }

        if (string.Compare(e.Data.Name, eventShootTorpedo) == 0)
        {
            Torpedo torpedo = PoolingController.Instance.poolTorpedo.New();

            if (torpedo == null)
            {
                torpedo = Instantiate(torpedoPrefab) as Torpedo;
            }

            float damage = ((SO_BossSubmarineStats)baseStats).Damage;
            float torpedoSpeed = ((SO_BossSubmarineStats)baseStats).BulletSpeed;

            AttackData atkData = new AttackData(this, damage);
            torpedo.Active(atkData, torpedoFirePoint, torpedoSpeed, PoolingController.Instance.groupBullet);

            ActiveDustMuzzle(torpedoFirePoint);

            SoundManager.Instance.PlaySfx(soundShootTorpedo);
        }
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        if (dieAnimationNames.Contains(entry.animation.name))
        {
            Deactive();
        }

        if (isDead)
            return;

        if (string.Compare(entry.animation.name, idleToRocket) == 0)
        {
            StartCoroutine(ReleaseRocket());
        }

        if (string.Compare(entry.animation.name, spawn) == 0)
        {
            StartCoroutine(SpawnMarine());
        }
    }

    private void ActiveDustMuzzle(Transform point)
    {
        if (dustMuzzle == null)
        {
            dustMuzzle = Instantiate<BaseMuzzle>(dustMuzzlePrefab, point.position, point.rotation, point);
        }

        dustMuzzle.Active();
    }

    private void MoveToBase()
    {
        if (isMovingToBase)
        {
            isMovingToBase = false;
            PlayAnimationIdle();

            transform.DOMove(basePosition, 4f).OnComplete(() =>
            {
                EventDispatcher.Instance.PostEvent(EventID.FinalBossStart);
                bodyCollider.gameObject.SetActive(true);
                ActiveRocket();

                float y = transform.position.y;
                y -= 1f;
                underWaterPosY = y;

            }).OnStart(() =>
            {
                CameraFollow.Instance.AddShake(0.3f, 3.5f);
            });
        }
    }

    private IEnumerator ReleaseTorpedo()
    {
        int count = 0;
        int totalTorpedo = ((SO_BossSubmarineStats)baseStats).NumberOfBullet;

        while (count < totalTorpedo)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, shootTorpedo, false);
            count++;
            yield return delayRocket;
        }

        DebugCustom.Log("ReleaseTorpedo, IDLE");
        PlayAnimationIdle();

        if (HpPercent > 0.5f)
        {
            nextAction = BossMarineAction.SpawnMarine;
            CheckEnableGore();
        }
        else
        {
            ActiveMarine();
        }
    }

    private IEnumerator ReleaseRocket()
    {
        int count = 0;
        int totalRocket = HpPercent > 0.5f ? ((SO_BossSubmarineStats)baseStats).NumberOfRocket : ((SO_BossSubmarineStats)baseStats).RageNumberOfRocket;

        while (count < totalRocket)
        {
            destinationRocket = target.transform.position;
            skeletonAnimation.AnimationState.SetAnimation(0, shootRocket, false);
            count++;
            yield return delayRocket;
        }

        PlayAnimationIdle();

        if (HpPercent > 0.5f)
        {
            nextAction = BossMarineAction.SpawnMarine;
            CheckEnableGore();
        }
        else
        {
            ActiveTorpedo();
        }
    }

    private IEnumerator SpawnMarine()
    {
        isSpawningMarines = true;
        int count = 0;
        int totalMarine = HpPercent > 0.5f ? ((SO_BossSubmarineStats)baseStats).NumberOfMarine : ((SO_BossSubmarineStats)baseStats).RageNumberOfMarine;

        while (count < totalMarine)
        {
            if (soundMarineAppear.Length > 0)
                SoundManager.Instance.PlaySfx(soundMarineAppear[Random.Range(0, soundMarineAppear.Length)]);

            EnemyMarine marine = PoolingController.Instance.poolEnemyMarine.New();

            if (marine == null)
            {
                marine = Instantiate(marinePrefab) as EnemyMarine;
            }

            marine.isInvisibleWhenActive = true;
            int marineLevel = HpPercent > 0.5f ? ((SO_BossSubmarineStats)baseStats).MarineLevel : ((SO_BossSubmarineStats)baseStats).RageMarineLevel;

            //if (GameData.mode == GameMode.Campaign)
            //{
            //    if (GameData.currentStage.difficulty == Difficulty.Hard)
            //    {
            //        marineLevel += StaticValue.LEVEL_INCREASE_MODE_HARD;
            //    }
            //    else if (GameData.currentStage.difficulty == Difficulty.Crazy)
            //    {
            //        marineLevel += StaticValue.LEVEL_INCREASE_MODE_CRAZY;
            //    }

            //    marineLevel = Mathf.Clamp(marineLevel, 1, StaticValue.MAX_LEVEL_ENEMY);
            //}

            marine.Active(marineLevel, marineSpawnPoint.position, underWaterPosY);
            marine.SetTarget(GameController.Instance.Player);

            GameController.Instance.AddUnit(marine.gameObject, marine);
            activeMarines.Add(marine.gameObject, marine);

            count++;
            yield return delaySpawnMarine;
        }

        isSpawningMarines = false;
        PlayAnimationIdle();
    }

    private void CheckEnableGore()
    {
        float distance = transform.position.x - target.transform.position.x;

        if (distance > 0 && distance < 5f)
        {
            ActiveGore();
        }
        else
        {
            ActiveNextAction();
        }
    }

    private void ActiveNextAction()
    {
        switch (nextAction)
        {
            case BossMarineAction.Rocket:
                ActiveRocket();
                break;

            case BossMarineAction.Torpedo:
                ActiveTorpedo();
                break;

            case BossMarineAction.SpawnMarine:
                ActiveMarine();
                break;
        }
    }

    private void ActiveRocket()
    {
        DebugCustom.Log("Active rocket");
        isReadyAttack = false;

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagRocket = true;
            flagTorpedo = false;
            flagMarine = false;
        },

        StaticValue.waitHalfSec));
    }

    private void ActiveTorpedo()
    {
        DebugCustom.Log("Active torpedo");
        isReadyAttack = false;

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagRocket = false;
            flagTorpedo = true;
            flagMarine = false;
        },

        StaticValue.waitHalfSec));
    }

    private void ActiveMarine()
    {
        DebugCustom.Log("Active marine");
        isReadyAttack = false;
        isSpawningMarines = true;

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagRocket = false;
            flagTorpedo = false;
            flagMarine = true;
        },

        StaticValue.waitHalfSec));
    }

    private void ActiveGore()
    {
        DebugCustom.Log("Active gore");
        isReadyAttack = false;
        flagRocket = false;
        flagTorpedo = false;
        flagMarine = false;

        StartCoroutine(DelayAction(() =>
        {
            float distance = transform.position.x;
            distance -= 3.5f;

            colliderGore.gameObject.SetActive(true);
            PlayAnimationIdle();

            transform.DOMoveX(distance, 1f).OnComplete(() =>
            {
                transform.DOMoveX(basePosition.x, 1.5f).OnComplete(() =>
                {
                    CheckEnableGore();
                });
            });
        },

        StaticValue.waitOneSec));
    }

    private void OnRubberBoatTriggerWater(Component senser, object param)
    {
        List<EnemyMarine> damagedMarines = new List<EnemyMarine>();

        Vector3 point = (Vector3)param;

        foreach (EnemyMarine marine in activeMarines.Values)
        {
            float deltaX = Mathf.Abs(marine.transform.position.x - point.x);
            float deltaY = point.y - marine.transform.position.y;

            if (deltaX <= 1.2f && deltaY <= 1.3f)
            {
                damagedMarines.Add(marine);
            }
        }

        for (int i = 0; i < damagedMarines.Count; i++)
        {
            int dmg = Random.Range(100, 150);
            damagedMarines[i].TakeDamage(dmg);
        }
    }

    private void OnUnitDie(Component senser, object param)
    {
        UnitDieData data = (UnitDieData)param;
        BaseEnemy enemy = data.unit.GetComponent<BaseEnemy>();

        if (activeMarines.ContainsKey(enemy.gameObject))
        {
            activeMarines.Remove(enemy.gameObject);

            if (activeMarines.Count == 1 && isSpawningMarines == false)
            {
                nextAction = BossMarineAction.Rocket;
                CheckEnableGore();
            }
        }
    }
}
