using UnityEngine;
using System.Collections;
using Spine.Unity;
using DG.Tweening;
using Spine;
using System.Collections.Generic;

public class BossMonkey : BaseEnemy
{
    private enum NextAction
    {
        None,
        ThrowStone,
        DropSpike,
        SpawnMinions
    }

    [Header("BOSS MONKEY PROPERTIES")]
    public BossMonkeyMinion minionPrefab;
    public BaseBullet stonePrefab;
    public Transform stoneStartPoint;
    public Vector2 stoneDirection;
    [SpineAnimation]
    public string throwStone, groundSlam, roar1, roar2, appear;
    [SpineEvent]
    public string eventThrowStone, eventSlam;
    public AudioClip soundAppear, soundIdle, soundSpawn, soundThrowStone;

    private NextAction nextAction;
    private bool flagMovingEntrance = true;
    [SerializeField]
    private bool flagStone;
    [SerializeField]
    private bool flagSpike;
    [SerializeField]
    private bool flagSpawn;
    [SerializeField]
    private bool flagChangeStateAfterSpawn;
    private int totalStone;
    private int countStone;
    private bool isSpikeHitPlayer;
    private Vector2 minionSpawnPoint;
    private Vector2 minionMostLeftPoint;
    private Vector2 minionMostRightPoint;
    [SerializeField]
    private Transform spawnSlamSmokePoint;

    private Dictionary<GameObject, BaseUnit> activeMinions = new Dictionary<GameObject, BaseUnit>();


    protected override void Start()
    {
        base.Start();

        EventDispatcher.Instance.RegisterListener(EventID.BossMonkeySpikeHitPlayer, (sender, param) => { isSpikeHitPlayer = true; });
        EventDispatcher.Instance.RegisterListener(EventID.BossMonkeySpikeTrapEnd, (sender, param) => OnSpikeTrapEnd());
        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, OnUnitDie);
    }

    protected override void Update()
    {
        if (isDead == false)
        {
            Entrance();

            if (isReadyAttack)
            {
                Attack();
            }
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_BOSS_MONKEY, level);
        baseStats = Resources.Load<SO_BossMonkeyStats>(path);
    }

    protected override void Attack()
    {
        if (flagStone)
        {
            flagStone = false;
            PlayAnimationThrow();
        }
        else if (flagSpike)
        {
            flagSpike = false;
            PlayAnimationSlam();
        }
        else if (flagSpawn)
        {
            flagSpawn = false;
            PlayAnimationRoar();
        }
    }

    protected override void StartDie()
    {
        base.StartDie();

        EventDispatcher.Instance.PostEvent(EventID.BossMonkeyDie);
    }

    public override void Renew()
    {
        isDead = false;

        LoadScriptableObject();
        stats.Init(baseStats);

        isFinalBoss = true;
        bodyCollider.enabled = false;
        isEffectMeleeWeapon = false;
        isReadyAttack = false;
        target = null;
        transform.parent = null;
        UpdateTransformPoints();
        UpdateHealthBar();
    }

    public override void UpdateHealthBar(bool isAutoHide = false)
    {
        UIController.Instance.hudBoss.SetIconBoss(id);
        UIController.Instance.hudBoss.UpdateHP(HpPercent);
    }

    public void SetPoints(Vector2 minionSpawnPoint, Vector2 minionMostLeftPoint, Vector2 minionMostRightPoint)
    {
        this.minionSpawnPoint = minionSpawnPoint;
        this.minionMostLeftPoint = minionMostLeftPoint;
        this.minionMostRightPoint = minionMostRightPoint;
    }

    private void Entrance()
    {
        if (flagMovingEntrance)
        {
            flagMovingEntrance = false;

            transform.DOMove(basePosition, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {

            }).OnStart(() =>
            {
                PlaySound(soundAppear);
                skeletonAnimation.AnimationState.SetAnimation(0, appear, false);
                CameraFollow.Instance.AddShake(1f, 1.5f);
            });
        }
    }

    private void OnSpikeTrapEnd()
    {
        if (nextAction == NextAction.SpawnMinions)
        {
            ActiveSpawn();
            nextAction = NextAction.None;
        }
        else
        {
            nextAction = NextAction.SpawnMinions;

            if (isSpikeHitPlayer)
            {
                isSpikeHitPlayer = false;
                ActiveStone(1);
            }
            else
            {
                ActiveSpike();
            }
        }

    }

    private void ActiveSoundIdle(bool isActive)
    {
        if (isActive)
        {
            if (audioSource.clip == null)
            {
                audioSource.loop = true;
                audioSource.clip = soundIdle;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    private void ActiveStone(int numberStone)
    {
        isReadyAttack = false;
        PlayAnimationIdle();
        countStone = 0;
        totalStone = numberStone;

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagStone = true;
            flagSpike = false;
            flagSpawn = false;
            ActiveSoundIdle(false);
        },

        StaticValue.waitHalfSec));
    }

    private void ActiveSpike()
    {
        isReadyAttack = false;
        PlayAnimationIdle();

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagStone = false;
            flagSpike = true;
            flagSpawn = false;
            ActiveSoundIdle(false);
        },

        StaticValue.waitHalfSec));
    }

    private void ActiveSpawn()
    {
        isReadyAttack = false;
        PlayAnimationIdle();

        StartCoroutine(DelayAction(() =>
        {
            isReadyAttack = true;
            flagStone = false;
            flagSpike = false;
            flagSpawn = true;
            ActiveSoundIdle(false);
        },

        StaticValue.waitHalfSec));
    }

    private IEnumerator SpawnMinions()
    {
        int count = 0;
        int totalMinions = 0;

        int minionLevel = ((SO_BossMonkeyStats)baseStats).LevelMinions;

        //if (GameData.mode == GameMode.Campaign)
        //{
        //    if (GameData.currentStage.difficulty == Difficulty.Hard)
        //    {
        //        minionLevel += StaticValue.LEVEL_INCREASE_MODE_HARD;
        //    }
        //    else if (GameData.currentStage.difficulty == Difficulty.Crazy)
        //    {
        //        minionLevel += StaticValue.LEVEL_INCREASE_MODE_CRAZY;
        //    }

        //    minionLevel = Mathf.Clamp(minionLevel, 1, StaticValue.MAX_LEVEL_ENEMY);
        //}

        if (HpPercent < 0.35f)
            totalMinions = ((SO_BossMonkeyStats)baseStats).Hp35_NumberMinions;
        else if (HpPercent < 0.65f)
            totalMinions = ((SO_BossMonkeyStats)baseStats).Hp65_NumberMinions;
        else
            totalMinions = ((SO_BossMonkeyStats)baseStats).NumberMinions;

        while (count < totalMinions)
        {
            BossMonkeyMinion minion = minionPrefab.GetFromPool() as BossMonkeyMinion;
            minion.isInvisibleWhenActive = true;
            minion.SetPoints(minionMostLeftPoint, minionMostRightPoint);
            minion.Active(minionPrefab.id, minionLevel, minionSpawnPoint);
            minion.SetTarget(GameController.Instance.Player);

            GameController.Instance.AddUnit(minion.gameObject, minion);
            activeMinions.Add(minion.gameObject, minion);

            count++;
            yield return StaticValue.waitHalfSec;
        }

        flagChangeStateAfterSpawn = true;
    }

    private void OnUnitDie(Component senser, object param)
    {
        UnitDieData data = (UnitDieData)param;
        BaseEnemy enemy = data.unit.GetComponent<BaseEnemy>();

        if (activeMinions.ContainsKey(enemy.gameObject))
        {
            CancelInvoke();
            activeMinions.Remove(enemy.gameObject);

            if (flagChangeStateAfterSpawn)
            {
                flagChangeStateAfterSpawn = false;

                bodyCollider.enabled = true;

                if (Random.Range(0, 2) == 0)
                {
                    nextAction = NextAction.DropSpike;
                    ActiveStone(2);
                }
                else
                {
                    ActiveSpike();
                }
            }
        }
    }


    #region SPINE & ANIMATIONS

    protected override void HandleAnimationStart(TrackEntry entry)
    {
        base.HandleAnimationStart(entry);
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        if (dieAnimationNames.Contains(entry.animation.name))
        {
            Deactive();
        }

        if (isDead)
            return;

        if (string.Compare(entry.animation.name, appear) == 0)
        {
            bodyCollider.enabled = true;
            skeletonAnimation.AnimationState.SetAnimation(0, idle, false);
            nextAction = NextAction.DropSpike;
            ActiveStone(2);
        }

        if (string.Compare(entry.animation.name, throwStone) == 0)
        {
            countStone++;

            if (countStone < totalStone)
            {
                this.StartDelayAction(() => { flagStone = true; }, 1f);
            }
            else
            {
                if (nextAction == NextAction.DropSpike)
                    ActiveSpike();
                else if (nextAction == NextAction.SpawnMinions)
                    ActiveSpawn();

                nextAction = NextAction.None;
            }

            PlayAnimationIdle();
        }

        if (string.Compare(entry.animation.name, groundSlam) == 0)
        {
            PlayAnimationIdle();

            DropSpikeData data = new DropSpikeData();
            data.boss = this;
            int numberSpikes = 0;
            float spikeDamage = 0;
            float spikeDropSpeed = 0;
            float spikeDelay = 0;

            if (HpPercent < 0.35f)
            {
                numberSpikes = ((SO_BossMonkeyStats)baseStats).Hp35_NumberSpikes;
                spikeDamage = ((SO_BossMonkeyStats)baseStats).Hp35_SpikeDamage;
                spikeDropSpeed = ((SO_BossMonkeyStats)baseStats).Hp35_SpikeSpeed;
                spikeDelay = ((SO_BossMonkeyStats)baseStats).Hp35_SpikeDelay;
            }
            else if (HpPercent < 0.65f)
            {
                numberSpikes = ((SO_BossMonkeyStats)baseStats).Hp65_NumberSpikes;
                spikeDamage = ((SO_BossMonkeyStats)baseStats).Hp65_SpikeDamage;
                spikeDropSpeed = ((SO_BossMonkeyStats)baseStats).Hp65_SpikeSpeed;
                spikeDelay = ((SO_BossMonkeyStats)baseStats).Hp65_SpikeDelay;
            }
            else
            {
                numberSpikes = ((SO_BossMonkeyStats)baseStats).NumberSpikes;
                spikeDamage = ((SO_BossMonkeyStats)baseStats).SpikeDamage;
                spikeDropSpeed = ((SO_BossMonkeyStats)baseStats).SpikeSpeed;
                spikeDelay = ((SO_BossMonkeyStats)baseStats).SpikeDelay;
            }

            data.numberSpikes = numberSpikes;
            data.spikeDamage = spikeDamage;
            data.spikeDropSpeed = spikeDropSpeed;
            data.spikeDelay = spikeDelay;

            EventDispatcher.Instance.PostEvent(EventID.BossMonkeySpikeTrapStart, data);
        }

        if (string.Compare(entry.animation.name, roar1) == 0
            || string.Compare(entry.animation.name, roar2) == 0)
        {
            bodyCollider.enabled = false;
            PlayAnimationIdle();
            StartCoroutine(SpawnMinions());
        }
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, eventThrowStone) == 0)
        {
            StoneBossMonkey stone = PoolingController.Instance.poolStoneBossMonkey.New();

            if (stone == null)
            {
                stone = Instantiate(stonePrefab) as StoneBossMonkey;
            }

            float stoneDamage = 0;
            if (HpPercent < 0.35f)
                stoneDamage = ((SO_BossMonkeyStats)baseStats).Hp35_StoneDamage;
            else if (HpPercent < 0.65f)
                stoneDamage = ((SO_BossMonkeyStats)baseStats).Hp65_StoneDamage;
            else
                stoneDamage = ((SO_BossMonkeyStats)baseStats).StoneDamage;

            AttackData atkData = new AttackData(this, stoneDamage);
            stone.Active(atkData, stoneStartPoint, target.BodyCenterPoint, stoneDirection);
        }

        if (string.Compare(e.Data.Name, eventSlam) == 0)
        {
            EffectController.Instance.SpawnParticleEffect(EffectObjectName.GroundSmoke, spawnSlamSmokePoint.position);
            CameraFollow.Instance.AddShake(0.2f, 0.3f);
        }
    }

    protected override void PlayAnimationThrow()
    {
        PlaySound(soundThrowStone);
        skeletonAnimation.AnimationState.SetAnimation(0, throwStone, false);
    }

    public override void PlayAnimationIdle()
    {
        base.PlayAnimationIdle();
        ActiveSoundIdle(true);
    }

    private void PlayAnimationSlam()
    {
        PlaySound(soundSpawn);
        skeletonAnimation.AnimationState.SetAnimation(0, groundSlam, false);
    }

    private void PlayAnimationRoar()
    {
        PlaySound(soundSpawn);
        CameraFollow.Instance.AddShake(0.2f, 2f);
        string animName = Random.Range(0, 2) == 0 ? roar1 : roar2;
        skeletonAnimation.AnimationState.SetAnimation(0, animName, false);
    }

    #endregion
}
