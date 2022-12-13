using UnityEngine;
using System.Collections;

public class BaseBulletPreview : MonoBehaviour
{
    protected float moveSpeed;

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    public virtual void Active(Transform firePoint, float moveSpeed, Transform parent = null)
    {
        this.moveSpeed = moveSpeed;

        transform.position = firePoint.position;
        transform.rotation = firePoint.rotation;
        transform.parent = parent;

        gameObject.SetActive(true);
    }

    protected virtual void Deactive()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(StaticValue.TAG_ENEMY))
        {
            EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);
        }

        SpawnHitEffect();
        Deactive();
    }

    protected virtual void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactNormal, transform.position);
    }
}
