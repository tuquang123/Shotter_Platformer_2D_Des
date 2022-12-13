using UnityEngine;

public class BaseGrenadeEnemy : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask layerVictim;

    protected bool isExploding;
    protected AttackData attackData;
    protected Collider2D[] victims = new Collider2D[10];

    private Rigidbody2D rigid;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        TrackOutOfScreen();
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_ENEMY) == false)
        {
            if (isExploding == false)
            {
                isExploding = true;
                Explode();
            }
        }
    }

    public virtual void Active(AttackData attackData, Vector3 startPoint, Vector3 endPoint, Vector2 throwDirection, Transform parent = null)
    {
        this.attackData = attackData;
        isExploding = false;
        transform.position = startPoint;
        transform.parent = parent;
        gameObject.SetActive(true);

        Vector3 direction = endPoint - startPoint;
        direction = MathUtils.ProjectVectorOnPlane(Vector3.up, direction);

        float angle = Vector2.Angle(throwDirection.x > 0 ? Vector3.right : Vector3.left, throwDirection);

        float yOffset = -direction.y;
        float currentSpeed = MathUtils.CalculateLaunchSpeed(direction.magnitude, yOffset, Physics2D.gravity.magnitude, angle * Mathf.Deg2Rad);

        rigid.velocity = throwDirection * currentSpeed;
    }

    public virtual void Deactive()
    {
        CancelInvoke();
        StopAllCoroutines();
        gameObject.SetActive(false);

        PoolingController.Instance.poolBaseGrenadeEnemy.Store(this);
    }

    public virtual void Explode()
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

        Deactive();
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeLarge, transform.position);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
        CameraFollow.Instance.AddShake(0.15f, 0.35f);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
    }

    private void TrackOutOfScreen()
    {
        bool isOutOfScreenX = transform.position.x < CameraFollow.Instance.left.position.x || transform.position.x > CameraFollow.Instance.right.position.x;
        bool isOutOfScreenY = transform.position.y < CameraFollow.Instance.bottom.position.y || transform.position.y > CameraFollow.Instance.top.position.y;

        if (isOutOfScreenX || isOutOfScreenY)
        {
            Deactive();
        }
    }
}
