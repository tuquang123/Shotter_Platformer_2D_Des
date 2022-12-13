using UnityEngine;
using System.Collections;

public class StoneBossMonkeyMinion : BaseBullet
{
    public AudioClip soundMoving;

    private Vector3 destination;

    protected override void Move()
    {
        Vector3 dir = destination - transform.position;
        transform.right = Vector3.MoveTowards(transform.right, dir.normalized, 4f * Time.deltaTime);
        transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
    }

    public override void Deactive()
    {
        base.Deactive();

        audioSource.Stop();
        PoolingController.Instance.poolStoneBossMonkeyMinion.Store(this);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.StoneBrokenSmall, transform.position);
        CameraFollow.Instance.AddShake(0.1f, 0.2f);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

            if (unit != null)
            {
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
        destination = endPoint.position;

        SetTagAndLayer();

        gameObject.SetActive(true);

        Vector3 direction = endPoint.position - startPoint.position;
        direction = MathUtils.ProjectVectorOnPlane(Vector3.up, direction);

        float angle = Vector2.Angle(throwDirection.x > 0 ? Vector3.right : Vector3.left, throwDirection);

        float yOffset = -direction.y;
        float currentSpeed = MathUtils.CalculateLaunchSpeed(direction.magnitude, yOffset, Physics2D.gravity.magnitude, angle * Mathf.Deg2Rad);

        rigid.velocity = throwDirection * currentSpeed;

        if (soundMoving)
        {
            audioSource.loop = true;
            audioSource.PlayOneShot(soundMoving);
        }
    }
}
