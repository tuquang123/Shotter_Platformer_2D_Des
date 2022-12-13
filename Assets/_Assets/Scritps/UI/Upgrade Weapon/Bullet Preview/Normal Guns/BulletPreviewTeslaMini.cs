using UnityEngine;
using System.Collections;

public class BulletPreviewTeslaMini : BaseBulletPreview
{
    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.teslaMini.Store(this);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactTeslaMini, transform.position);
    }
}
