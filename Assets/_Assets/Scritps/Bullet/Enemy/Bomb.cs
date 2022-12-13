using UnityEngine;
using System.Collections;

public class Bomb : BaseBullet
{
    protected override void Move() { }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, transform.position);
        CameraFollow.Instance.AddShake(0.15f, 0.35f);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
    }

    public override void Active(AttackData attackData, Transform releasePoint, float moveSpeed, Transform parent = null)
    {
        this.attackData = attackData;
        this.moveSpeed = moveSpeed;

        SetTagAndLayer();

        transform.position = releasePoint.position;
        transform.rotation = releasePoint.rotation;
        transform.parent = parent;

        gameObject.SetActive(true);

        rigid.AddForce(Random.onUnitSphere * 1000f);
        rigid.AddTorque(250f);
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletBomb.Store(this);
    }
}
