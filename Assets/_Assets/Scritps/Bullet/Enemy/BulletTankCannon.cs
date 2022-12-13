using UnityEngine;
using System.Collections;

public class BulletTankCannon : BaseBullet
{
    public AudioClip soundExplode;
    public LayerMask layerVictim;

    private Vector3 destinationRocket;
    private Collider2D[] victims = new Collider2D[5];

    protected override void Move()
    {
        Vector3 dir = destinationRocket - transform.position;
        transform.right = Vector3.MoveTowards(transform.right, dir.normalized, 4f * Time.deltaTime);
        transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletTankCannon.Store(this);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, transform.position);
        CameraFollow.Instance.AddShake(0.15f, 0.35f);

        SoundManager.Instance.PlaySfx(soundExplode);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, attackData.radiusDealDamage, victims, layerVictim);

        for (int i = 0; i < count; i++)
        {
            BaseUnit unit = GameController.Instance.GetUnit(victims[i].transform.root.gameObject); // victims[i].transform.root.GetComponent<BaseUnit>();

            if (unit != null && unit.CompareTag(StaticValue.TAG_PLAYER))
            {
                float distance = Vector3.Distance(transform.position, unit.BodyCenterPoint.position);
                float distancePercent = Mathf.Clamp01((distance - 0.5f) / (attackData.radiusDealDamage - 0.5f));
                float damagePercentByDistance = 1 - (distancePercent * 0.4f);
                attackData.damage *= damagePercentByDistance;
                unit.TakeDamage(attackData);
            }
        }

        SpawnHitEffect();
        Deactive();
    }

    public void Active(AttackData attackData, Transform startPoint, Transform endPoint, Vector2 throwDirection)
    {
        this.attackData = attackData;

        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
        destinationRocket = endPoint.position;

        SetTagAndLayer();

        gameObject.SetActive(true);

        Vector3 direction = endPoint.position - startPoint.position;
        direction = MathUtils.ProjectVectorOnPlane(Vector3.up, direction);

        float angle = Vector2.Angle(throwDirection.x > 0 ? Vector3.right : Vector3.left, throwDirection);

        float yOffset = -direction.y;
        float currentSpeed = MathUtils.CalculateLaunchSpeed(direction.magnitude, yOffset, Physics2D.gravity.magnitude, angle * Mathf.Deg2Rad);

        rigid.velocity = throwDirection * currentSpeed;
    }
}
