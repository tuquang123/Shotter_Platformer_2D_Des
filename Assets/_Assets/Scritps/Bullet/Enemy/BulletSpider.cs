using UnityEngine;
using System.Collections;

public class BulletSpider : BaseBullet
{
    public float amplitude = 0.5f;
    public float rate = 0.5f;

    private Vector3 startPos;

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletSpider.Store(this);
    }

    protected override void Move()
    {
        float theta = Time.timeSinceLevelLoad / rate;
        float distance = amplitude * Mathf.Sin(theta);

        startPos += transform.right * moveSpeed * Time.deltaTime;
        transform.position = startPos + transform.up * distance;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

            if (unit != null)
            {
                unit.TakeDamage(attackData);
                SpawnHitEffect();
                Deactive();
            }
        }
    }

    public override void Active(AttackData attackData, Transform releasePoint, float moveSpeed, Transform parent = null)
    {
        base.Active(attackData, releasePoint, moveSpeed, parent);

        startPos = releasePoint.position;
    }
}
