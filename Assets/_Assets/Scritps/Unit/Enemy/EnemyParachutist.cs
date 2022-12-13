using UnityEngine;
using System.Collections;
using System.Linq;
using Spine.Unity;
using Spine;

public class EnemyParachutist : BaseEnemy
{
    [Header("ENEMY PARACHUTIST PROPERTIES")]
    public Transform groundUnderCheckPoint;
    [SpineAnimation]
    public string parachute;
    public MeshRenderer[] frontWeaponParts;
    public MeshRenderer[] behindWeaponParts;
    public BaseGunEnemy[] gunPrefabs;

    [SerializeField]
    private bool isParachuting;
    private BaseGunEnemy gun;


    protected override void Update()
    {
        if (isDead == false)
        {
            if (isParachuting)
                CheckGround();

            UpdateDirection();
            TrackAimPoint();

            if (isParachuting == false)
            {
                Idle();
                Patrol();
            }

            Attack();
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_PARACHUTIST, level);
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

            if (isParachuting == false)
                GetCloseToTarget();

            float currentTime = Time.time;

            if (currentTime - lastTimeAttack > stats.AttackRate)
            {
                lastTimeAttack = currentTime;
                PlayAnimationShoot();
            }
        }
    }

    protected override void ReleaseAttack()
    {
        base.ReleaseAttack();
        gun.Attack(this);
    }

    protected override void StartDie()
    {
        base.StartDie();

        rigid.bodyType = RigidbodyType2D.Dynamic;
    }

    protected override void PlayAnimationShoot(int trackIndex = 1)
    {
        TrackEntry track = null;

        if (isParachuting)
        {
            track = skeletonAnimation.AnimationState.SetAnimation(1, shoot, false);
        }
        else
        {
            if (flagGetCloseToTarget)
            {
                track = skeletonAnimation.AnimationState.SetAnimation(1, shoot, false);
            }
            else
            {
                track = skeletonAnimation.AnimationState.SetAnimation(0, shoot, false);
            }
        }

        track.AttachmentThreshold = 1f;
        track.MixDuration = 0f;
        TrackEntry empty = skeletonAnimation.AnimationState.AddEmptyAnimation(1, 0.5f, 0.1f);
        empty.AttachmentThreshold = 1f;
    }

    protected override void CancelCombat()
    {
        target = null;

        if (isParachuting)
        {
            PlayAnimationParachute();
        }
        else
        {
            SwitchState(EnemyState.Idle);
        }
    }

    protected override void TrackAimPoint()
    {
        if (target)
        {
            bool isAim = Mathf.Abs(transform.position.x - target.transform.position.x) >= 0.7f;
            ActiveAim(isAim);

            if (isParachuting)
            {
                aimBone.transform.position = target.BodyCenterPoint.position;
            }
            else
            {
                aimBone.transform.position = aimPoint.position;
            }
        }
        else
        {
            ActiveAim(false);
        }
    }

    public override void OnUnitGetInFarSensor(BaseUnit unit)
    {
        SetTarget(unit);

        if (isParachuting)
            return;

        if (Vector2.Distance(target.transform.position, BodyCenterPoint.position) > nearSensor.col.radius)
        {
            if (canMove)
            {
                flagGetCloseToTarget = true;
                PlayAnimationMoveFast();
            }
        }
    }

    public override void OnUnitGetOutFarSensor(BaseUnit unit)
    {
        if (isParachuting)
        {
            CancelCombat();
            return;
        }

        if (canMove)
        {
            farSensor.gameObject.SetActive(false);
            StartCoroutine(DelayAction(() =>
            {
                farSensor.gameObject.SetActive(true);
                flagGetCloseToTarget = false;
                StartChasingTarget();
            },

            StaticValue.waitHalfSec));
        }
        else
        {
            CancelCombat();
        }
    }

    public override void OnUnitGetInNearSensor(BaseUnit unit)
    {
        if (isParachuting)
            return;

        if (canMove)
        {
            flagGetCloseToTarget = false;
            PlayAnimationIdle();
            StopMoving();
        }
    }

    public override void OnUnitGetOutNearSensor(BaseUnit unit)
    {
        if (isParachuting)
            return;

        if (canMove)
        {
            nearSensor.gameObject.SetActive(false);
            StartCoroutine(DelayAction(() =>
            {
                nearSensor.gameObject.SetActive(true);
                flagGetCloseToTarget = true;
                flagMove = true;
                PlayAnimationMoveFast();
            },

            StaticValue.waitHalfSec));
        }
    }

    public override void Renew()
    {
        base.Renew();

        rigid.bodyType = RigidbodyType2D.Kinematic;
        PlayAnimationParachute();
        isParachuting = true;
    }

    public override BaseEnemy GetFromPool()
    {
        EnemyParachutist unit = PoolingController.Instance.poolEnemyParachutist.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyParachutist;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemyParachutist.Store(this);
    }

    private void CheckGround()
    {
        bool isGrounded = Physics2D.Linecast(transform.position, groundUnderCheckPoint.position, layerMaskCheckObstacle);

        if (isGrounded)
        {
            isParachuting = false;
            rigid.bodyType = RigidbodyType2D.Dynamic;
            skeletonAnimation.AnimationState.SetAnimation(0, idle, false);
            UpdateDirection();
            ResetAim();
        }

        if (isParachuting)
        {
            transform.Translate(Vector3.down * 1f * Time.deltaTime, Space.World);
        }
    }

    private void PlayAnimationParachute()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, parachute, true);
    }
}
