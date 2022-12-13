using UnityEngine;
using System.Collections;
using Spine;
using UnityEngine.Events;
using System.Linq;
using Spine.Unity;

public class EnemyGrenade : BaseEnemy
{
    [Header("ENEMY GRENADE PROPERTIES")]
    public MeshRenderer[] frontWeaponParts;
    public MeshRenderer[] behindWeaponParts;
    public BaseGunEnemy[] gunPrefabs;
    public BaseGrenadeEnemy grenadePrefab;
    public Transform throwStartPoint;
    public Vector2 throwDirection;
    public GameObject grenade;
    [SpineAnimation]
    public string idleNade, idleGun;

    private BaseGunEnemy gun;
    private bool isUsingGun;
    [SerializeField]
    private bool flagThrow;
    private int bulletShot;
    private int grenadeThrew;
    private float lastTimeThrowGrenade;
    private Vector2 destinationThrow;



    protected override void Update()
    {
        if (isDead == false)
        {
            UpdateDirection();
            //TrackAimPoint();
            Idle();
            Patrol();
            Attack();
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_GRENADE, level);
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
        grenade.GetComponent<SpriteRenderer>().sortingOrder = randomSortingLayer + 1;

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

            if (isReadyAttack && !flagThrow)
            {
                float currentTime = Time.time;

                if (isUsingGun)
                {
                    if (currentTime - lastTimeAttack > stats.AttackRate)
                    {
                        lastTimeAttack = currentTime;
                        PlayAnimationShoot();
                    }
                }
                else
                {
                    if (currentTime - lastTimeThrowGrenade > 3f)
                    {
                        lastTimeThrowGrenade = currentTime;
                        flagThrow = true;
                        destinationThrow = target.transform.position;
                        PlayAnimationThrow();
                    }
                }
            }
        }
    }

    protected override void Die()
    {
        base.Die();

        EventDispatcher.Instance.PostEvent(EventID.KillEnemyGrenade);
    }

    protected override void ReleaseAttack()
    {
        base.ReleaseAttack();
        gun.Attack(this);
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

        if (string.Compare(entry.animation.name, shoot) == 0)
        {
            bulletShot++;

            if (bulletShot >= 3)
            {
                isReadyAttack = false;
                StartCoroutine(DelayAction(() => SwitchWeapon(false), StaticValue.waitHalfSec));
            }
        }

        if (string.Compare(entry.animation.name, throwGrenade) == 0)
        {
            flagThrow = false;
            grenadeThrew++;

            if (grenadeThrew >= 2)
            {
                isReadyAttack = false;
                StartCoroutine(DelayAction(() => SwitchWeapon(true), StaticValue.waitHalfSec));
            }

            skeletonAnimation.AnimationState.SetEmptyAnimation(2, 0f);
        }

        if (string.Compare(entry.animation.name, aim) == 0)
        {
            Invoke("ReadyToAttack", 0.5f);
        }
    }

    protected override void TrackAimPoint()
    {
        if (target)
        {
            bool isAim = Mathf.Abs(transform.position.x - target.transform.position.x) >= 0.7f && isUsingGun;
            ActiveAim(isAim);
        }
        else
        {
            ActiveAim(false);
        }
    }

    protected override void ActiveAim(bool isActive)
    {
        if (isActive)
        {
            if (skeletonAnimation.AnimationState.GetCurrent(1) != null
                && string.Compare(skeletonAnimation.AnimationState.GetCurrent(1).animation.name, aim) == 0)
                return;

            skeletonAnimation.AnimationState.SetAnimation(1, aim, false);
        }
        else
        {
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
        }
    }

    protected override void PlayAnimationShoot(int trackIndex = 1)
    {
        skeletonAnimation.AnimationState.SetAnimation(2, shoot, false);
    }

    protected override void PlayAnimationThrow()
    {
        skeletonAnimation.AnimationState.SetAnimation(2, throwGrenade, false);
    }

    public override BaseEnemy GetFromPool()
    {
        EnemyGrenade unit = PoolingController.Instance.poolEnemyGrenade.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyGrenade;
        }

        return unit;
    }

    public override void Renew()
    {
        base.Renew();

        SwitchWeapon(false);
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemyGrenade.Store(this);
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

    private void SwitchWeapon(bool isUsingGun)
    {
        this.isUsingGun = isUsingGun;
        gun.gameObject.SetActive(isUsingGun);
        grenade.SetActive(!isUsingGun);
        bulletShot = 0;
        grenadeThrew = 0;
        flagThrow = false;
        ActiveAim(isUsingGun);
        Invoke("ReadyToAttack", 1f);
    }

    public override void PlayAnimationIdle()
    {
        TrackEntry track = skeletonAnimation.AnimationState.GetCurrent(0);
        idle = isUsingGun ? idleGun : idleNade;

        if (track == null || string.Compare(track.animation.name, idle) != 0)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, idle, true);
        }
    }
}
