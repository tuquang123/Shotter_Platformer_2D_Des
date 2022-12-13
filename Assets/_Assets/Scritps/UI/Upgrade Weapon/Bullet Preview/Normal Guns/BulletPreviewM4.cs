using UnityEngine;
using System.Collections;

public class BulletPreviewM4 : BaseBulletPreview
{
    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.m4.Store(this);
    }
}
