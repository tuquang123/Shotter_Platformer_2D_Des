using System.Collections;
using UnityEngine;

public class BaseGrenade : BaseWeapon
{
    [Header("BASE STATS")]
    public SO_GrenadeStats baseStats;

    [Header("BASE GRENADE PROPERTIES")]
    public BaseEffect effectTextPrefab;
    public LayerMask layerVictim;

    protected bool isExploding;
    protected AttackData attackData;
    protected Collider2D[] victims = new Collider2D[10];

    private Rigidbody2D rigid;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_ENEMY))
        {
            if (isExploding == false)
            {
                isExploding = true;
                Explode();
            }
        }
        else if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER) == false)
        {
            if (isExploding == false)
            {
                isExploding = true;
                StartCoroutine(DelayExplode());
            }
        }
    }

    private IEnumerator DelayExplode()
    {
        yield return StaticValue.waitHalfSec;

        Explode();
    }

    private void SpawnEffectText()
    {
        if (effectTextPrefab == null)
            return;

        EffectTextBANG textBang = PoolingController.Instance.poolTextBANG.New();

        if (textBang == null)
        {
            textBang = Instantiate(effectTextPrefab) as EffectTextBANG;
        }

        Vector2 v = transform.position;
        v.y += 1.5f;

        textBang.Active(v);
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

    public virtual BaseGrenade Create()
    {
        BaseGrenade grenade = PoolingController.Instance.poolBaseGrenade.New();

        if (grenade == null)
        {
            grenade = Instantiate(this) as BaseGrenade;
        }

        return grenade;
    }

    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GRENADE, level);
        baseStats = Resources.Load<SO_GrenadeStats>(path);
    }

    public override void Init(int level)
    {
        base.Init(level);
    }

    public override void ApplyOptions(BaseUnit unit) { }

    public override void Attack(AttackData attackData) { }

    public virtual void Active(AttackData attackData, Vector3 startPoint, Vector2 throwForce, Transform parent = null)
    {
        this.attackData = attackData;
        isExploding = false;
        transform.position = startPoint;
        transform.parent = parent;
        gameObject.SetActive(true);

        rigid.AddForce(throwForce, ForceMode2D.Impulse);
    }

    public virtual void Deactive()
    {
        CancelInvoke();
        StopAllCoroutines();
        gameObject.SetActive(false);

        PoolingController.Instance.poolBaseGrenade.Store(this);
    }

    public virtual void Explode()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, attackData.radiusDealDamage, victims, layerVictim);
        int enemyKilled = 0;

        for (int i = 0; i < count; i++)
        {
            BaseUnit unit = null;

            if (victims[i].CompareTag(StaticValue.TAG_ENEMY_BODY_PART) || victims[i].CompareTag(StaticValue.TAG_DESTRUCTIBLE_OBSTACLE))
            {
                unit = GameController.Instance.GetUnit(victims[i].gameObject);
            }
            else if (victims[i].transform.root.CompareTag(StaticValue.TAG_ENEMY))
            {
                unit = GameController.Instance.GetUnit(victims[i].transform.root.gameObject);
            }

            if (unit)
            {
                float distance = Vector3.Distance(transform.position, unit.BodyCenterPoint.position);
                float distancePercent = Mathf.Clamp01((distance - 0.5f) / (attackData.radiusDealDamage - 0.5f));
                float damagePercentByDistance = 1 - (distancePercent * 0.4f);
                attackData.damage *= damagePercentByDistance;
                unit.TakeDamage(attackData);

                if (unit.CompareTag(StaticValue.TAG_ENEMY) && unit.isDead)
                {
                    enemyKilled++;
                }
            }
        }

        EventDispatcher.Instance.PostEvent(EventID.GrenadeKillEnemyAtOnce, enemyKilled);

        Deactive();
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeLarge, transform.position);
        SpawnEffectText();
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
        CameraFollow.Instance.AddShake(0.15f, 0.35f);
    }
}
