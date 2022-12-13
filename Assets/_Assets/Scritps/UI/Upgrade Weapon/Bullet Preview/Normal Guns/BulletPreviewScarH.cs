using UnityEngine;
using System.Collections;

public class BulletPreviewScarH : BaseBulletPreview
{
    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.scarH.Store(this);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeSmall, transform.position);
    }
}
