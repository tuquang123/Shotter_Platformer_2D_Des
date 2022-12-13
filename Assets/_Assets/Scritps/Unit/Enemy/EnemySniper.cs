using UnityEngine;
using System.Collections;
using Spine;
using System.Linq;
using Spine.Unity;

public class EnemySniper : BaseEnemy
{
    [Header("ENEMY SNIPER PROPERTIES")]
    public MeshRenderer[] frontWeaponParts;
    public MeshRenderer[] behindWeaponParts;
    public BaseGunEnemy[] gunPrefabs;
    public BaseMeleeWeaponEnemy[] knifePrefabs;
    public WindBlade windEffect;

    private GunEnemyAWP gun;
    private BaseMeleeWeaponEnemy knife;
    private bool isUsingGun;
    [SerializeField]
    private bool flagKnife;
    private float lastTimeKnife;


    protected override void Update()
    {
        if (isDead == false)
        {
            UpdateDirection();
            TrackAimPoint();
            Idle();
            Attack();

            if (gun)
                gun.ActiveLaserAim(target);
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_SNIPER, level);
        baseStats = Resources.Load<SO_EnemySniperStats>(path);
    }

    protected override void InitWeapon()
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

        // Gun
        if (gunPrefabs.Length <= 0)
        {
            DebugCustom.Log("Gun prefabs length = 0");
        }
        else
        {
            if (index > gunPrefabs.Length - 1)
                index = 0;

            gun = Instantiate(gunPrefabs[index], transform) as GunEnemyAWP;
            gun.Active(this);
        }

        // Knife
        if (knifePrefabs.Length <= 0)
        {
            DebugCustom.Log("Knife prefabs length = 0");
        }
        else
        {
            if (index > knifePrefabs.Length - 1)
                index = 0;

            knife = Instantiate(knifePrefabs[index], transform);
            knife.Active(this);
        }
    }

    protected override void InitSortingLayerSpine()
    {
        int randomSortingLayer = Random.Range(200, 700);
        gun.spr.sortingOrder = randomSortingLayer;
        knife.GetComponent<MeshRenderer>().sortingOrder = randomSortingLayer;

        for (int i = 0; i < frontWeaponParts.Length; i++)
        {
            frontWeaponParts[i].sortingOrder = randomSortingLayer + 1;
        }

        for (int i = 0; i < behindWeaponParts.Length; i++)
        {
            behindWeaponParts[i].sortingOrder = randomSortingLayer - 1;
        }
    }

    protected override void TrackAimPoint()
    {
        if (target)
        {
            bool isAim = Mathf.Abs(transform.position.x - target.transform.position.x) >= 0.7f && flagKnife == false;
            ActiveAim(isAim);

            if (aimBone != null)
                aimBone.transform.position = target.BodyCenterPoint.position;
        }
        else
        {
            ActiveAim(false);
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

            float currentTime = Time.time;

            if (isUsingGun)
            {
                if (flagKnife)
                    return;

                if (currentTime - lastTimeAttack > stats.AttackRate)
                {
                    lastTimeAttack = currentTime;
                    PlayAnimationShoot();
                }
            }
            else
            {
                if (flagKnife)
                    return;

                if (flagGetCloseToTarget == false)
                {
                    float knifeRate = 1f / ((SO_EnemySniperStats)baseStats).KnifeAttackTimePerSecond;

                    if (currentTime - lastTimeKnife > knifeRate)
                    {
                        lastTimeKnife = currentTime;
                        flagKnife = true;
                        skeletonAnimation.AnimationState.SetAnimation(1, meleeAttack, false);
                        windEffect.Active(true);
                    }
                }
            }
        }
    }

    protected override void Die()
    {
        base.Die();

        EventDispatcher.Instance.PostEvent(EventID.KillEnemySniper);
    }

    protected override void ReleaseAttack()
    {
        base.ReleaseAttack();
        gun.Attack(this);
    }

    protected override void SetCloseRange()
    {
        if (nearSensor != null)
        {
            if (nearSensor != null)
            {
                closeUpRange = Random.Range(0.9f, 1.25f);
                nearSensor.col.radius = closeUpRange;
            }
        }
    }

    public override void TakeDamage(AttackData attackData)
    {
        base.TakeDamage(attackData);

        if (isDead)
        {
            if (attackData.weaponId != -1)
            {
                StaticGunData gun = GameData.staticGunData.GetData(attackData.weaponId);

                if (gun != null && gun.id == StaticValue.GUN_ID_AWP)
                {
                    EventDispatcher.Instance.PostEvent(EventID.KillEnemySniperByGunAWP);
                }
            }
        }
    }

    public override BaseEnemy GetFromPool()
    {
        EnemySniper unit = PoolingController.Instance.poolEnemySniper.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemySniper;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemySniper.Store(this);
    }

    public override void Renew()
    {
        base.Renew();

        flagKnife = false;
        SwitchWeapon(true);
        gun.ActiveLaserAim(false);
    }

    public override void OnUnitGetInNearSensor(BaseUnit unit)
    {
        flagGetCloseToTarget = false;
        PlayAnimationIdle();
        StopMoving();

        if (nearbyVictims.Contains(unit) == false)
            nearbyVictims.Add(unit);

        SwitchWeapon(false);
    }

    public override void OnUnitGetOutNearSensor(BaseUnit unit)
    {
        isReadyAttack = false;
        SwitchWeapon(true);
        StartCoroutine(DelayAction(ReadyToAttack, StaticValue.waitOneSec));
    }

    public override void OnUnitGetInFarSensor(BaseUnit unit)
    {
        SetTarget(unit);
        flagGetCloseToTarget = false;
    }

    public override void OnUnitGetOutFarSensor(BaseUnit unit)
    {
        CancelCombat();
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        base.HandleAnimationEvent(trackEntry, e);

        if (string.Compare(e.Data.Name, eventMeleeAttack) == 0)
        {
            if (isDead == false)
            {
                for (int i = 0; i < nearbyVictims.Count; i++)
                {
                    AttackData atkData = GetCurentAttackData();
                    nearbyVictims[i].TakeDamage(atkData);
                }
            }
        }
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        base.HandleAnimationCompleted(entry);

        if (isDead)
            return;

        if (string.Compare(entry.animation.name, meleeAttack) == 0)
        {
            flagKnife = false;
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0);
        }
    }

    private void SwitchWeapon(bool isUsingGun)
    {
        this.isUsingGun = isUsingGun;
        gun.gameObject.SetActive(isUsingGun);
        knife.gameObject.SetActive(!isUsingGun);
        ResetAim();
    }
}
