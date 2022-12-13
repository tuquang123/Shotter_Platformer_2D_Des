using UnityEngine;
using System.Collections;

public class BulletShotgun : BaseBullet
{
    public float distance;

    private Vector2 startPoint;

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletShotgun.Store(this);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        BaseUnit unit = null;

        if (other.CompareTag(StaticValue.TAG_ENEMY_BODY_PART) || other.CompareTag(StaticValue.TAG_DESTRUCTIBLE_OBSTACLE))
        {
            unit = GameController.Instance.GetUnit(other.gameObject);
        }
        else if (other.transform.root.CompareTag(StaticValue.TAG_ENEMY))
        {
            unit = GameController.Instance.GetUnit(other.transform.root.gameObject);
        }

        if (unit != null)
            unit.TakeDamage(attackData);

        //SpawnHitEffect();
        //Deactive();
    }

    public override void Active(AttackData attackData, Transform releasePoint, float moveSpeed, Transform parent = null)
    {
        base.Active(attackData, releasePoint, moveSpeed, parent);

        startPoint = transform.position;
    }

    protected override void TrackingDeactive()
    {
        if (Vector2.Distance(transform.position, startPoint) >= distance)
        {
            Deactive();
            return;
        }
    }

    protected override void SpawnHitEffect() { }
}
