using UnityEngine;
using System.Collections;

public class BulletPreviewSpread : BaseBulletPreview
{
    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.spread.Store(this);
    }
}
