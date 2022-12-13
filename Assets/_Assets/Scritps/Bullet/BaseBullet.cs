using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public bool isTrackingLifeTime;
    public bool isDeactiveWhenOutScreen = true;
    public float lifeTime = 2f;

    protected float timerLifeTime;
    protected float moveSpeed;
    protected Transform target;
    protected AttackData attackData;
    protected Rigidbody2D rigid;
    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        Move();
        TrackingDeactive();
    }

    protected virtual void Move()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    protected virtual void TrackingDeactive()
    {
        if (isTrackingLifeTime && GameData.mode == GameMode.Survival)
        {
            timerLifeTime += Time.deltaTime;

            if (timerLifeTime >= lifeTime)
            {
                timerLifeTime = 0;
                Deactive();
                return;
            }
        }

        if (isDeactiveWhenOutScreen)
        {
            bool isOutOfScreenX = transform.position.x < CameraFollow.Instance.left.position.x - 0.7f || transform.position.x > CameraFollow.Instance.right.position.x + 0.7f;
            //bool isOutOfScreenY = transform.position.y < CameraFollow.Instance.bottom.position.y || transform.position.y > CameraFollow.Instance.top.position.y;
            bool isOutOfScreenY = transform.position.y > CameraFollow.Instance.top.position.y + 0.7f;

            if (isOutOfScreenX || isOutOfScreenY)
            {
                Deactive();
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        switch (tag)
        {
            case StaticValue.TAG_BULLET_RAMBO:
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

                    break;
                }

            case StaticValue.TAG_BULLET_ENEMY:
                if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
                {
                    BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

                    if (unit != null)
                    {
                        unit.TakeDamage(attackData);
                        EventDispatcher.Instance.PostEvent(EventID.EnemyShootHitPlayer, attackData);
                    }
                }
                break;
        }

        SpawnHitEffect();
        Deactive();
    }

    protected virtual void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactNormal, transform.position);
    }

    protected virtual void SetTagAndLayer()
    {
        if (attackData == null)
            return;

        switch (attackData.attacker.tag)
        {
            case StaticValue.TAG_PLAYER:
                tag = StaticValue.TAG_BULLET_RAMBO;
                gameObject.layer = StaticValue.LAYER_BULLET_RAMBO;
                break;

            case StaticValue.TAG_ENEMY:
                tag = StaticValue.TAG_BULLET_ENEMY;
                gameObject.layer = StaticValue.LAYER_BULLET_ENEMY;
                break;

            default:
                tag = StaticValue.TAG_NONE;
                gameObject.layer = StaticValue.LAYER_DEFAULT;
                break;
        }
    }

    public virtual void Active(AttackData attackData, Transform releasePoint, float moveSpeed, Transform parent = null)
    {
        this.attackData = attackData;
        this.moveSpeed = moveSpeed;

        SetTagAndLayer();

        transform.position = releasePoint.position;
        transform.rotation = releasePoint.rotation;
        transform.parent = parent;

        gameObject.SetActive(true);
    }

    public virtual void Deactive()
    {
        timerLifeTime = 0;

        gameObject.SetActive(false);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
