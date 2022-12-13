using UnityEngine;
using System.Collections;
using Spine;
using System.Linq;
using Spine.Unity;
using System.Collections.Generic;

public class EnemyKnife : BaseEnemy
{
    [Header("ENEMY KNIFE PROPERTIES")]
    public MeshRenderer[] frontWeaponParts;
    public MeshRenderer[] behindWeaponParts;
    public WindBlade windEffect;
    [SpineAnimation]
    public string jumpForward, idleShield, meleeAttackShield, walkShield, runShield;
    [SpineAnimation(startsWith = "die")]
    public List<string> dieShieldAnimationNames;
    public BaseMeleeWeaponEnemy[] knifePrefabs;

    private BaseMeleeWeaponEnemy knife;
    private bool flagKnife;


    protected override void Update()
    {
        if (isDead == false)
        {
            UpdateDirection();
            Idle();
            Patrol();
            Attack();
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_ENEMY_KNIFE, level);
        baseStats = Resources.Load<SO_BaseUnitStats>(path);
    }

    protected override void InitSkin()
    {
        string skin = defaultSkin;

        if (GameData.mode == GameMode.Campaign)
        {
            int mapId = int.Parse(GameController.Instance.CampaignMap.stageNameId.Split('.').First());

            switch ((MapType)mapId)
            {
                case MapType.Map_1_Desert:
                    if (!string.IsNullOrEmpty(skinMap1))
                        skin = skinMap1;
                    break;

                case MapType.Map_2_Lab:
                    if (!string.IsNullOrEmpty(skinMap2))
                    {
                        skin = skinMap2;
                        idle = idleShield;
                        meleeAttack = meleeAttackShield;
                        move = walkShield;
                        moveFast = runShield;
                        dieAnimationNames = dieShieldAnimationNames;
                    }
                    break;

                case MapType.Map_3_Jungle:
                    if (!string.IsNullOrEmpty(skinMap3))
                        skin = skinMap3;
                    break;
            }
        }
        else if (GameData.mode == GameMode.Survival)
        {
            int random = Random.Range(0, 3);

            if (random == 0)
            {
                if (!string.IsNullOrEmpty(skinMap1))
                    skin = skinMap1;
            }
            else if (random == 1)
            {
                if (!string.IsNullOrEmpty(skinMap2))
                {
                    skin = skinMap2;
                    idle = idleShield;
                    meleeAttack = meleeAttackShield;
                    move = walkShield;
                    moveFast = runShield;
                    dieAnimationNames = dieShieldAnimationNames;
                }
            }
            else if (random == 2)
            {
                if (!string.IsNullOrEmpty(skinMap3))
                    skin = skinMap3;
            }
        }

        skeletonAnimation.Skeleton.SetSkin(skin);
    }

    protected override void InitWeapon()
    {
        if (knifePrefabs.Length <= 0)
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
                index = Random.Range(0, knifePrefabs.Length);
            }

            if (index > knifePrefabs.Length - 1)
                index = 0;

            knife = Instantiate(knifePrefabs[index], transform);
            knife.Active(this);
        }
    }

    protected override void InitSortingLayerSpine()
    {
        int randomSortingLayer = Random.Range(200, 700);
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

    protected override void Attack()
    {
        if (state == EnemyState.Attack)
        {
            if (target == null || target.isDead)
            {
                CancelCombat();
                return;
            }

            if (flagKnife) return;

            GetCloseToTarget();

            if (flagGetCloseToTarget == false)
            {
                float currentTime = Time.time;

                if (currentTime - lastTimeAttack > stats.AttackRate)
                {
                    lastTimeAttack = currentTime;
                    flagKnife = true;
                    PlayAnimationMeleeAttack();
                    windEffect.Active(true);
                }
            }
        }
    }

    protected override void SetCloseRange()
    {
        if (nearSensor != null)
        {
            if (nearSensor != null)
            {
                closeUpRange = Random.Range(1f, 1.6f);
                nearSensor.col.radius = closeUpRange;
            }
        }
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, eventMeleeAttack) == 0)
        {
            if (isDead == false)
            {
                PlaySound(soundAttack);

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

    public override void Renew()
    {
        base.Renew();

        flagKnife = false;
        canJump = true;
    }

    public override void Active(EnemySpawnData spawnData)
    {
        base.Active(spawnData);

        canMove = true;
        canJump = true;
    }

    protected override void Die()
    {
        base.Die();

        EventDispatcher.Instance.PostEvent(EventID.KillEnemyKnife);
    }

    public override BaseEnemy GetFromPool()
    {
        EnemyKnife unit = PoolingController.Instance.poolEnemyKnife.New();

        if (unit == null)
        {
            unit = Instantiate(this) as EnemyKnife;
        }

        return unit;
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolEnemyKnife.Store(this);
    }
}
