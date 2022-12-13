using UnityEngine;
using System.Collections;

public class RocketBossMegatank : BaseBullet
{
    public AudioClip soundExplode;

    private bool isHitPlayer;
    private Vector3 destinationRocket;


    protected override void Move()
    {
        Vector3 dir = destinationRocket - transform.position;
        transform.right = Vector3.MoveTowards(transform.right, dir.normalized, 4f * Time.deltaTime);
        transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolRocketBossMegatank.Store(this);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, transform.position);
        CameraFollow.Instance.AddShake(0.15f, 0.35f);

        SoundManager.Instance.PlaySfx(soundExplode);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        switch (tag)
        {
            case StaticValue.TAG_BULLET_ENEMY:
                if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
                {
                    BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

                    if (unit != null)
                    {
                        unit.TakeDamage(attackData);
                        isHitPlayer = true;
                    }
                }
                break;
        }

        SpawnHitEffect();

        if (isHitPlayer)
        {
            EventDispatcher.Instance.PostEvent(EventID.RocketMegatankHitPlayer);
        }
        else
        {
            EventDispatcher.Instance.PostEvent(EventID.RocketMegatankMissPlayer);
        }

        Deactive();
    }

    public void Active(AttackData attackData, Transform startPoint, Transform endPoint, Vector2 throwDirection)
    {
        this.attackData = attackData;

        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
        destinationRocket = endPoint.position;
        isHitPlayer = false;

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
