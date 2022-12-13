using UnityEngine;
using System.Collections;

public class BaseItemDrop : MonoBehaviour
{
    protected Rigidbody2D rigid;
    protected ItemDropData data;
    protected Collider2D col;
    protected SpriteRenderer spr;


    protected virtual void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            EventDispatcher.Instance.PostEvent(EventID.GetItemDrop, data);
            Deactive();
        }
    }

    public virtual void Active(ItemDropData data, Vector2 position)
    {
        this.data = data;
        transform.position = position;
        col.enabled = true;
        gameObject.SetActive(true);
    }

    public virtual void Deactive()
    {
        col.enabled = false;
        gameObject.SetActive(false);
    }
}
