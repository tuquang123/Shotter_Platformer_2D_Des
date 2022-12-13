using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletPreviewFlame : BaseBulletPreview
{
    public float timeApplyDamage = 0.3f;
    public Collider2D col;
    public GameObject subFlame1;
    public GameObject subFlame2;

    private bool isActive;
    private List<Transform> victims = new List<Transform>();
    private float lastTimeDealDamage;

    protected override void Update()
    {
        float currentTime = Time.time;
        float deltaTime = currentTime - lastTimeDealDamage;
        if (deltaTime > timeApplyDamage)
        {
            lastTimeDealDamage = currentTime;
            DealDamage();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag(StaticValue.TAG_ENEMY))
        {
            if (victims.Contains(other.transform) == false)
            {
                victims.Add(other.transform);
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag(StaticValue.TAG_ENEMY))
        {
            if (victims.Contains(other.transform))
            {
                victims.Remove(other.transform);
            }
        }
    }

    private void DealDamage()
    {
        if (victims.Count <= 0)
            return;

        EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);
    }

    public void ActiveSubFlame1()
    {
        if (subFlame1.activeSelf == false)
        {
            subFlame1.SetActive(true);
        }
    }

    public void ActiveSubFlame2()
    {
        if (subFlame2.activeSelf == false)
        {
            subFlame2.SetActive(true);
        }
    }
}
