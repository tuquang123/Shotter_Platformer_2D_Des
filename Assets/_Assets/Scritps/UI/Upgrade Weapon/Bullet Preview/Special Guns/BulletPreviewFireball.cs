using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletPreviewFireball : BaseBulletPreview
{
    public float timeApplyDamage;
    public float distance;

    private Vector2 startPoint;
    private float timerDamage;

    private List<GameObject> victims = new List<GameObject>();

    protected override void Move()
    {
        if (Vector2.Distance(transform.position, startPoint) >= distance)
        {
            Deactive();
            return;
        }

        base.Move();

        if (transform.localScale.x <= 2)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * 2f, 2f * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        ApplyDamage();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(StaticValue.TAG_ENEMY))
        {
            if (victims.Contains(other.transform.root.gameObject) == false)
            {
                victims.Add(other.transform.root.gameObject);
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(StaticValue.TAG_ENEMY))
        {
            if (victims.Contains(other.transform.root.gameObject))
            {
                victims.Remove(other.transform.root.gameObject);
            }
        }
    }

    public override void Active(Transform firePoint, float moveSpeed, Transform parent = null)
    {
        base.Active(firePoint, moveSpeed, parent);

        startPoint = transform.position;
        transform.localScale = Vector3.one;
        timerDamage = 0;
        victims.Clear();
    }

    private void ApplyDamage()
    {
        if (victims.Count <= 0)
            return;

        timerDamage += Time.deltaTime;

        if (timerDamage >= timeApplyDamage)
        {
            timerDamage = 0;
            EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);
        }
    }

    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.fireball.Store(this);
    }
}
