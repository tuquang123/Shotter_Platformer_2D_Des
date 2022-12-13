using UnityEngine;
using System.Collections;
using Spine.Unity;
using DG.Tweening;
using Spine;

public class EnemyMarine : BaseEnemy
{
    [Header("ENEMY MARINE PROPERTIES")]
    [SpineAnimation]
    public string jumpForward;

    private float underWaterY;
    private bool isAppearDone;
    private bool flagAttack;


    protected override void Start()
    {
        base.Start();

        EventDispatcher.Instance.RegisterListener(EventID.BossSubmarineDie, (sender, param) => Deactive());
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_MARINE, level);
        baseStats = Resources.Load<SO_BaseUnitStats>(path);
    }

    protected override void Update()
    {
        if (isDead == false)
        {
            if (isReadyAttack)
            {
                Attack();
            }
        }
    }

    protected override void Attack()
    {
        if (state == EnemyState.Attack && isAppearDone)
        {
            if (target == null || target.isDead)
            {
                CancelCombat();
                return;
            }

            if (flagAttack) return;

            UpdateDirection();

            if (IsTargetInCloseRange())
            {
                float currentTime = Time.time;

                if (currentTime - lastTimeAttack > stats.AttackRate)
                {
                    lastTimeAttack = currentTime;
                    PlayAnimationIdle();
                    StopMoving();

                    flagAttack = true;
                    skeletonAnimation.AnimationState.SetAnimation(1, meleeAttack, false);
                }
            }
            else
            {
                PlayAnimationMove();
                Move();
            }
        }
    }

    protected override void CheckAllowMoveForwardToTarget() { }

    protected override bool IsTargetInCloseRange()
    {
        if (target != null)
        {
            float sqrDistance = (target.transform.position - BodyCenterPoint.position).sqrMagnitude;
            bool heightEnough = Mathf.Abs(target.transform.position.y - BodyCenterPoint.position.y) < 1.2f;
            return sqrDistance < 1.3f && heightEnough;
        }

        return false;
    }

    public void Active(int level, Vector2 position, float underWaterY)
    {
        base.Active(id, level, position);

        this.underWaterY = underWaterY;
    }

    public override void Renew()
    {
        base.Renew();

        ActiveSensor(false);
        flagAttack = false;
        isAppearDone = false;
        isImmortal = true;
    }

    public override BaseEnemy GetFromPool()
    {
        EnemyMarine unit = PoolingController.Instance.poolEnemyMarine.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyMarine;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemyMarine.Store(this);
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        base.HandleAnimationCompleted(entry);

        if (isDead)
            return;

        if (string.Compare(entry.animation.name, meleeAttack) == 0)
        {
            flagAttack = false;
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0);
        }

        if (string.Compare(entry.animation.name, jumpForward) == 0)
        {
            PlayAnimationIdle();

            rigid.bodyType = RigidbodyType2D.Kinematic;
            bodyCollider.gameObject.SetActive(false);
            footCollider.gameObject.SetActive(false);

            transform.DOMoveY(underWaterY, 1f).OnComplete(() =>
            {
                rigid.bodyType = RigidbodyType2D.Dynamic;
                bodyCollider.gameObject.SetActive(true);
                footCollider.gameObject.SetActive(true);
                isAppearDone = true;
                isReadyAttack = true;
                isImmortal = false;
            });
        }
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, eventMeleeAttack) == 0)
        {
            if (IsTargetInCloseRange())
            {
                if (target.transform.root.CompareTag(StaticValue.TAG_PLAYER))
                {
                    BaseUnit unit = GameController.Instance.GetUnit(target.transform.root.gameObject);

                    if (unit != null)
                    {
                        unit.TakeDamage(GetCurentAttackData());
                    }
                }
            }
        }
    }

    protected override void FadeIn()
    {
        DOTween.To(AlphaSetter, 0f, 1f, 0.5f).OnComplete(FadeInDone);
    }

    protected override void FadeInDone()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, jumpForward, false);
    }

}
