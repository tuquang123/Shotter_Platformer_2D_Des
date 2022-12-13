using UnityEngine;
using System.Collections;

public class BulletPreviewUzi : BaseBulletPreview
{
    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.uzi.Store(this);
    }
}
