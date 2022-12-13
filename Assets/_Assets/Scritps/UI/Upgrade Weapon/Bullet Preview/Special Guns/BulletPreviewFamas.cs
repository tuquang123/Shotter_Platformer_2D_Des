using UnityEngine;
using System.Collections;

public class BulletPreviewFamas : BaseBulletPreview
{
    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.famas.Store(this);
    }
}
