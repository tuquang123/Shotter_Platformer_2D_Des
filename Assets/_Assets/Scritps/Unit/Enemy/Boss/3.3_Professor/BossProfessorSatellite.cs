using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;

public class BossProfessorSatellite : BaseEnemy
{
    [Header("SATELLITE PROPERTIES")]
    public float hp;
    [SpineAnimation]
    public string die, preShoot;

    private BossProfessor boss;
    private bool isShooting;

    protected override void Awake()
    {
        bodyCollider = GetComponent<CircleCollider2D>();
        bodyCollider.enabled = false;
        boss = transform.root.GetComponent<BossProfessor>();
        GameController.Instance.AddUnit(gameObject, this);
    }

    protected override void Update() { }

    public void Init()
    {
        bodyCollider.enabled = true;
        hp = ((SO_BossProfessorStats)boss.baseStats).SatelliteHp;
    }

    public void Shoot()
    {
        if (isDead == false)
        {
            if (isShooting == false)
            {
                isShooting = true;
                skeletonAnimation.AnimationState.SetAnimation(1, preShoot, false);
            }
        }
    }

    public override void Deactive()
    {
        PlaySoundDie();
        bodyCollider.enabled = false;
        skeletonAnimation.ClearState();
        skeletonAnimation.AnimationState.SetAnimation(0, die, false);
        GameController.Instance.RemoveUnit(gameObject);
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, transform.position);
    }

    protected override void ReleaseAttack()
    {
        //base.ReleaseAttack();

        BulletBossProfessor bullet = PoolingController.Instance.poolBulletBossProfessor.New();

        if (bullet == null)
        {
            bullet = Instantiate(boss.bulletSatellitePrefab) as BulletBossProfessor;
        }

        float damage = ((SO_BossProfessorStats)boss.baseStats).Damage;
        float bulletSpeed = ((SO_BossProfessorStats)boss.baseStats).BulletSpeed;
        AttackData atkData = new AttackData(boss, damage);

        bullet.Active(atkData, transform, bulletSpeed, PoolingController.Instance.groupBullet);
    }

    protected override void Die()
    {
        isDead = true;
        Deactive();
        EventDispatcher.Instance.PostEvent(EventID.BossProfessorSatelliteDie);
    }

    public override void TakeDamage(AttackData attackData)
    {
        if (isDead || attackData.attacker.isDead)
            return;

        EffectTakeDamage();
        hp -= attackData.damage;

        if (hp <= 0)
        {
            hp = 0;
            Die();
        }
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, eventShoot) == 0)
        {
            ReleaseAttack();
        }
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        if (isDead)
            return;

        if (string.Compare(entry.animation.name, shoot) == 0)
        {
            isShooting = false;
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0);
        }

        if (string.Compare(entry.animation.name, preShoot) == 0)
        {
            skeletonAnimation.AnimationState.SetAnimation(1, shoot, false);
        }
    }
}
