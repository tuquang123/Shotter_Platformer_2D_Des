using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;
using DG.Tweening;

public class BossMonkeyMinion : BaseEnemy
{
    [Header("MONKEY MINION PROPERTIES")]
    public BaseBullet stonePrefab;
    public Transform stoneStartPoint;
    public Vector2 stoneDirection;
    public Transform healthBarLeft;
    public Transform healthBarRight;
    [SpineAnimation]
    public string throwStone;
    [SpineEvent]
    public string eventThrowStone;
    public AudioClip soundAppear;

    private bool flagThrow;
    private bool flagEntrance;
    private Vector2 mostLeftPoint;
    private Vector2 mostRightPoint;
    private Vector2 standPosition;

    protected override void Start()
    {
        base.Start();

        EventDispatcher.Instance.RegisterListener(EventID.BossMonkeyDie, (sender, param) => Deactive());
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_BOSS_MONKEY_MINION, level);
        baseStats = Resources.Load<SO_BaseUnitStats>(path);
    }

    protected override void Update()
    {
        if (isDead == false)
        {
            Entrance();

            if (isReadyAttack)
            {
                UpdateDirection();
                Attack();
            }
        }
    }

    protected override void Attack()
    {
        if (state == EnemyState.Attack)
        {
            if (target == null || target.isDead)
            {
                CancelCombat();
                return;
            }

            if (flagThrow) return;

            float currentTime = Time.time;

            if (currentTime - lastTimeAttack > stats.AttackRate)
            {
                lastTimeAttack = currentTime;
                flagThrow = true;
                PlayAnimationThrow();
            }
        }
    }

    protected override void UpdateDirection()
    {
        if (target)
        {
            skeletonAnimation.Skeleton.flipX = (target.transform.position.x < transform.position.x);
        }

        UpdateTransformPoints();
    }

    protected override void UpdateTransformPoints()
    {
        base.UpdateTransformPoints();

        healthBar.transform.parent.position = skeletonAnimation.Skeleton.flipX ? healthBarLeft.position : healthBarRight.position;
    }

    public void SetPoints(Vector2 mostLeftPoint, Vector2 mostRightPoint)
    {
        this.mostLeftPoint = mostLeftPoint;
        this.mostRightPoint = mostRightPoint;

        Vector2 v = mostLeftPoint;
        v.x = Random.Range(mostLeftPoint.x, mostRightPoint.x);
        standPosition = v;
    }

    public override void Renew()
    {
        base.Renew();

        ActiveSensor(false);

        isReadyAttack = false;
        isImmortal = true;
        flagEntrance = true;
        flagThrow = false;

        PlaySound(soundAppear);
    }

    public override BaseEnemy GetFromPool()
    {
        BossMonkeyMinion unit = PoolingController.Instance.poolBossMonkeyMinion.New();

        if (unit == null)
        {
            unit = Instantiate(this) as BossMonkeyMinion;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBossMonkeyMinion.Store(this);
    }

    private void Entrance()
    {
        if (flagEntrance)
        {
            flagEntrance = false;
            PlayAnimationMove();
            skeletonAnimation.Skeleton.flipX = (standPosition.x < transform.position.x);

            float s = Mathf.Abs(standPosition.x - transform.position.x);
            float v = baseStats.MoveSpeed;
            float t = s / v;

            transform.DOMove(standPosition, t).SetEase(Ease.Linear).OnComplete(() =>
            {
                isImmortal = false;
                isReadyAttack = true;
            });
        }
    }

    #region SPINE & ANIMATION
    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        base.HandleAnimationCompleted(entry);

        if (isDead)
            return;

        if (string.Compare(entry.animation.name, throwStone) == 0)
        {
            flagThrow = false;
            PlayAnimationIdle();
        }
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, eventThrowStone) == 0)
        {
            StoneBossMonkeyMinion stone = PoolingController.Instance.poolStoneBossMonkeyMinion.New();

            if (stone == null)
            {
                stone = Instantiate(stonePrefab) as StoneBossMonkeyMinion;
            }

            AttackData atkData = new AttackData(this, baseStats.Damage);
            stone.Active(atkData, stoneStartPoint, target.BodyCenterPoint, stoneDirection);
        }
    }

    protected override void PlayAnimationThrow()
    {
        PlaySound(soundAttack);
        skeletonAnimation.AnimationState.SetAnimation(0, throwStone, false);
    }

    #endregion
}
