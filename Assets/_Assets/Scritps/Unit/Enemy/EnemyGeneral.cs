using UnityEngine;
using System.Collections;
using Spine;
using System.Linq;

public class EnemyGeneral : BaseEnemy
{
    [Header("ENEMY GENERAL PROPERTIES")]
    public MeshRenderer[] frontWeaponParts;
    public MeshRenderer[] behindWeaponParts;
    public BaseGunEnemy[] gunPrefabs;
    public BaseGrenadeEnemy grenadePrefab;
    public Transform throwStartPoint;
    public Vector2 throwDirection;
    public float timeSwitchGrenade = 3f;

    private BaseGunEnemy gun;
    [SerializeField]
    private bool flagThrow;
    private float timeCheckThrowGrenade;
    private Vector2 destinationThrow;


    protected override void Awake()
    {
        base.Awake();

        EventDispatcher.Instance.RegisterListener(EventID.EnemyShootHitPlayer, OnShootHitPlayer);
    }

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
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_GENERAL, level);
        baseStats = Resources.Load<SO_EnemyHasProjectileStats>(path);
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

    protected override void TrackAimPoint()
    {
        if (target)
        {
            bool isAim = Mathf.Abs(transform.position.x - target.transform.position.x) >= 0.7f && flagThrow == false;
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

            if (flagThrow)
                return;

            GetCloseToTarget();

            float currentTime = Time.time;
            //float deltaTime = currentTime - timeCheckThrowGrenade;

            if (currentTime - timeCheckThrowGrenade > timeSwitchGrenade)
            {
                timeCheckThrowGrenade = Time.time;
                flagThrow = true;
                destinationThrow = target.transform.position;
                PlayAnimationThrow();
                return;
            }

            if (currentTime - lastTimeAttack > stats.AttackRate)
            {
                lastTimeAttack = currentTime;
                PlayAnimationShoot();
            }
        }
    }

    protected override void Die()
    {
        base.Die();

        EventDispatcher.Instance.PostEvent(EventID.KillEnemyGeneral);
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        base.HandleAnimationEvent(trackEntry, e);

        if (string.Compare(e.Data.Name, eventThrowGrenade) == 0)
        {
            if (isDead == false)
            {
                ThrowGrenade(throwStartPoint.position, destinationThrow);
            }
        }
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        base.HandleAnimationCompleted(entry);

        if (isDead)
            return;

        if (string.Compare(entry.animation.name, throwGrenade) == 0)
        {
            flagThrow = false;
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
        }
    }

    protected override void ReleaseAttack()
    {
        base.ReleaseAttack();
        gun.Attack(this);
    }

    public override void Renew()
    {
        base.Renew();

        flagThrow = false;
    }

    public override void SetTarget(BaseUnit unit)
    {
        base.SetTarget(unit);

        timeCheckThrowGrenade = Time.time;
    }

    public override BaseEnemy GetFromPool()
    {
        EnemyGeneral unit = PoolingController.Instance.poolEnemyGeneral.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyGeneral;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemyGeneral.Store(this);
    }

    private void ThrowGrenade(Vector3 startPoint, Vector3 endPoint)
    {
        BaseGrenadeEnemy grenade = PoolingController.Instance.poolBaseGrenadeEnemy.New();

        if (grenade == null)
        {
            grenade = Instantiate(grenadePrefab) as BaseGrenadeEnemy;
        }

        float grenadeDamage = ((SO_EnemyHasProjectileStats)baseStats).ProjectileDamage;
        float grenadeRadius = ((SO_EnemyHasProjectileStats)baseStats).ProjectileDamageRadius;
        AttackData atkData = new AttackData(this, grenadeDamage, grenadeRadius);

        Vector2 v = throwDirection;
        v.x = IsFacingRight ? v.x : -v.x;

        grenade.Active(atkData, startPoint, endPoint, v, PoolingController.Instance.groupGrenade);

        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_THROW_GRENADE);
    }

    private void OnShootHitPlayer(Component sender, object param)
    {
        AttackData atkData = (AttackData)param;

        if (ReferenceEquals(atkData.attacker, this))
        {
            timeCheckThrowGrenade = Time.time;
        }
    }
}
