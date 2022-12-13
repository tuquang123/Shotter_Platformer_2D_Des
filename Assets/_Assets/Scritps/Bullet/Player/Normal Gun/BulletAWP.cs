using UnityEngine;
using System.Collections;

public class BulletAWP : BaseBullet
{
    private int hitEnemies;

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletAWP.Store(this);
    }

    public override void Active(AttackData attackData, Transform releasePoint, float moveSpeed, Transform parent = null)
    {
        base.Active(attackData, releasePoint, moveSpeed, parent);

        hitEnemies = 0;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        BaseUnit unit = null;

        if (other.CompareTag(StaticValue.TAG_ENEMY_BODY_PART) || other.CompareTag(StaticValue.TAG_DESTRUCTIBLE_OBSTACLE))
        {
            unit = GameController.Instance.GetUnit(other.gameObject);
        }
        else if (other.transform.root.CompareTag(StaticValue.TAG_ENEMY))
        {
            unit = GameController.Instance.GetUnit(other.transform.root.gameObject);
        }

        if (unit != null)
        {
            attackData.damage *= (1 - hitEnemies * 0.15f);
            unit.TakeDamage(attackData);
            hitEnemies++;

            if (hitEnemies >= 3)
                Deactive();
        }

        SpawnHitEffect();
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeSmall, transform.position);
    }
}
