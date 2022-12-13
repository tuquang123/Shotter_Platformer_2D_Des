using UnityEngine;
using System.Collections;

public class BulletPreviewShotgun : BaseBulletPreview
{
    public float distance;

    private Vector2 startPoint;

    protected override void Move()
    {
        if (Vector2.Distance(transform.position, startPoint) >= distance)
        {
            Deactive();
            return;
        }

        base.Move();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(StaticValue.TAG_ENEMY))
        {
            EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);
            //Deactive();
        }
    }

    public override void Active(Transform firePoint, float moveSpeed, Transform parent = null)
    {
        base.Active(firePoint, moveSpeed, parent);

        startPoint = transform.position;
    }

    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.shotgun.Store(this);
    }
}
