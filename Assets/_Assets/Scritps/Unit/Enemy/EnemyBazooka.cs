using UnityEngine;
using System.Collections;
using System.Linq;
using Spine;

public class EnemyBazooka : BaseEnemy
{
    [Header("ENEMY BAZOOKA PROPERTIES")]
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
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_BAZOOKA, level);
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
            behindWeaponParts[i].sortingOrder = randomSortingLayer;
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

            //GetCloseToTarget();

            if (isReadyAttack)
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

        if (string.Compare(entry.animation.name, shoot) == 0)
        {
            PlayAnimationIdle();
        }
    }

    //protected override void PlayAnimationShoot(int trackIndex = 1)
    //{
    //    skeletonAnimation.AnimationState.SetAnimation(0, shoot, false);
    //}

    public override BaseEnemy GetFromPool()
    {
        EnemyBazooka unit = PoolingController.Instance.poolEnemyBazooka.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyBazooka;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemyBazooka.Store(this);
    }

    public override void OnUnitGetInFarSensor(BaseUnit unit)
    {
        SetTarget(unit);
        PlayAnimationIdle();
    }
}
