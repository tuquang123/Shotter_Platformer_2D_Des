using UnityEngine;
using System.Collections;

public class BulletPreviewBullpup : BaseBulletPreview
{
    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.bullpup.Store(this);
    }
}
