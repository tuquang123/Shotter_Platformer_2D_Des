using UnityEngine;
using System.Collections;

public class BulletPreviewSniperRifle : BaseBulletPreview
{
    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.sniperRifle.Store(this);
    }
}
