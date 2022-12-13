using UnityEngine;
using System.Collections;

public class BulletPreviewP100 : BaseBulletPreview
{
    public SpriteRenderer sprRenderer;

    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.p100.Store(this);
    }

    protected override void Move()
    {
        base.Move();

        sprRenderer.transform.Rotate(0f, 0f, 1000f * Time.deltaTime);
    }
}
