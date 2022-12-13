using UnityEngine;
using System.Collections;

public class BaseGrenadePreview : MonoBehaviour
{
    public int id;
    public SO_GrenadeStats baseStats;
    public LayerMask layerVictim;

    protected bool isExploding;
    protected Collider2D[] victims = new Collider2D[2];

    private Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(StaticValue.TAG_ENEMY))
        {
            if (isExploding == false)
            {
                isExploding = true;
                Explode();
            }
        }
        else if (other.gameObject.CompareTag(StaticValue.TAG_PLAYER) == false)
        {
            if (isExploding == false)
            {
                isExploding = true;
                StartCoroutine(DelayExplode());
            }
        }
    }

    public virtual void Deactive()
    {
        CancelInvoke();
        StopAllCoroutines();

        gameObject.SetActive(false);
    }

    public virtual void Active(Vector3 startPoint, Vector2 throwForce, Transform parent = null)
    {
        isExploding = false;
        transform.position = startPoint;
        transform.parent = parent;
        gameObject.SetActive(true);

        rigid.AddForce(throwForce, ForceMode2D.Impulse);
    }

    protected virtual void Explode()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, baseStats.Radius, victims, layerVictim);

        if (count > 0)
        {
            EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);
        }

        Deactive();
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeLarge, transform.position);
    }

    protected IEnumerator DelayExplode()
    {
        yield return StaticValue.waitOneSec;

        Explode();
    }
}
