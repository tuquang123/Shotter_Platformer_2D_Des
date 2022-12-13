using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;

public class BaseMuzzle : MonoBehaviour
{
    public bool isDeactiveByTime = true;

    private float timeDeactive = 0.1f;
    private SpriteRenderer sprite;
    private float timer;
    private bool isActive;

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (isActive && isDeactiveByTime)
        {
            timer += Time.deltaTime;

            if (timer >= timeDeactive)
            {
                timer = 0;
                Deactive();
            }
        }
    }

    public virtual void Active()
    {
        timer = 0;

        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>();

        if (sprite)
            sprite.enabled = true;

        isActive = true;
        gameObject.SetActive(true);
    }

    public virtual void Deactive()
    {
        timer = 0;

        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>();

        if (sprite)
            sprite.enabled = false;

        isActive = false;
        gameObject.SetActive(false);
    }
}
