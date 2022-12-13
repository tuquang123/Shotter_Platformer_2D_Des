using UnityEngine;
using System.Collections;

public class BulletKamePower : BaseBullet
{
    private float percentCharge;

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletKamePower.Store(this);
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
            unit.TakeDamage(attackData);
            SpawnHitEffect();
            Deactive();
        }
    }

    public virtual void Active(AttackData attackData, Transform releasePoint, float moveSpeed, float percentCharge, Transform parent = null)
    {
        this.attackData = attackData;
        this.moveSpeed = moveSpeed;
        this.percentCharge = percentCharge;

        SetTagAndLayer();

        transform.position = releasePoint.position;
        transform.rotation = releasePoint.rotation;
        transform.localScale = Vector3.one * percentCharge;
        transform.parent = parent;

        gameObject.SetActive(true);
    }

    protected override void SpawnHitEffect()
    {
        if (percentCharge >= 0.95f)
        {
            CameraFollow.Instance.AddShake(0.2f, 0.2f);
            EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeLarge, transform.position);
        }
        else if (percentCharge >= 0.7f)
        {
            EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, transform.position);
        }
        else
        {
            EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactNormal, transform.position);
        }
    }
}
