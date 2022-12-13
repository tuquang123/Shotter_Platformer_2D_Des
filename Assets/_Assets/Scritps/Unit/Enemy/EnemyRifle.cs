using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;
using System.Linq;

public class EnemyRifle : BaseEnemy
{
    [Header("ENEMY RIFLE PROPERTIES")]
    public bool isHideOnBush;
    public BaseGunEnemy[] gunPrefabs;
    public MeshRenderer[] frontWeaponParts;
    public MeshRenderer[] behindWeaponParts;

    private BaseGunEnemy gun;


    protected override void Update()
    {
        if (isDead == false)
        {
            UpdateDirection();
            TrackAimPoint();
            Idle();
            Patrol();
            Attack();
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_RIFLE, level);
        baseStats = Resources.Load<SO_BaseUnitStats>(path);
    }

    protected override void InitWeapon()
    {
        if (gunPrefabs.Length <= 0)
        {
            DebugCustom.Log("Gun prefabs length = 0");
        }
        else
        {
            int index = 0;

            if (GameData.mode == GameMode.Campaign)
            {
                int mapId = int.Parse(GameController.Instance.CampaignMap.stageNameId.Split('.').First());
                index = mapId - 1;
            }
            else if (GameData.mode == GameMode.Survival)
            {
                index = Random.Range(0, gunPrefabs.Length);
            }

            if (index > gunPrefabs.Length - 1)
                index = 0;

            gun = Instantiate(gunPrefabs[index], transform);
            gun.Active(this);
        }
    }

    protected override void InitSortingLayerSpine()
    {
        int randomSortingLayer = Random.Range(200, 700);
        gun.spr.sortingOrder = randomSortingLayer;

        for (int i = 0; i < frontWeaponParts.Length; i++)
        {
            frontWeaponParts[i].sortingOrder = randomSortingLayer + 1;
        }

        for (int i = 0; i < behindWeaponParts.Length; i++)
        {
            behindWeaponParts[i].sortingOrder = randomSortingLayer - 1;
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

            GetCloseToTarget();
            CheckAllowAttackTarget();

            if (isAllowAttackTarget && isReadyAttack)
            {
                float currentTime = Time.time;

                if (currentTime - lastTimeAttack > stats.AttackRate)
                {
                    lastTimeAttack = currentTime;
                    PlayAnimationShoot();
                }
            }
        }
    }

    protected override void ReleaseAttack()
    {
        base.ReleaseAttack();
        gun.Attack(this);
    }

    protected override void Die()
    {
        base.Die();

        EventDispatcher.Instance.PostEvent(EventID.KillEnemyRifle);
    }

    protected override void ActiveAim(bool isActive)
    {
        if (isActive)
        {
            if (skeletonAnimation.AnimationState.GetCurrent(2) != null
                && string.Compare(skeletonAnimation.AnimationState.GetCurrent(2).animation.name, aim) == 0)
                return;

            skeletonAnimation.AnimationState.SetAnimation(2, aim, false);
        }
        else
        {
            skeletonAnimation.AnimationState.SetEmptyAnimation(2, 0f);
            isReadyAttack = false;
            ResetAim();
        }
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        base.HandleAnimationCompleted(entry);

        if (isDead)
            return;

        if (string.Compare(entry.animation.name, aim) == 0)
        {
            Invoke("ReadyToAttack", 0.5f);
        }
    }

    public override BaseEnemy GetFromPool()
    {
        EnemyRifle unit = PoolingController.Instance.poolEnemyRifle.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyRifle;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemyRifle.Store(this);
    }
}
