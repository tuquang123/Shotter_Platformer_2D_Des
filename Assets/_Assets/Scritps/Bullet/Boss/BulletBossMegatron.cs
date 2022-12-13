using UnityEngine;
using System.Collections;

public class BulletBossMegatron : BaseBullet
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletBossMegatron.Store(this);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, transform.position);
        CameraFollow.Instance.AddShake(0.3f, 0.5f);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
    }

    //protected override void TrackingDeactive()
    //{
    //    bool isOutOfScreenX = transform.position.x < CameraFollow.Instance.left.position.x || transform.position.x > CameraFollow.Instance.right.position.x;
    //    bool isOutOfScreenY = transform.position.y < CameraFollow.Instance.bottom.position.y || transform.position.y > CameraFollow.Instance.top.position.y;

    //    if (isOutOfScreenX || isOutOfScreenY)
    //    {
    //        Deactive();
    //    }
    //}
}
