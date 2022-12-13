using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoisonTrap : MonoBehaviour
{
    [HideInInspector]
    public bool isActive;

    private BoxCollider2D col;
    private List<BaseUnit> victims = new List<BaseUnit>();

    public float lifeTime = 5f;
    public float damage = 5f;
    public float timeApplyDamage = 1f;
    public float slowPercent = 0.5f;

    private float timeOut;
    private float lastTimeDealDamage;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isActive)
        {
            timeOut += Time.deltaTime;

            if (timeOut >= lifeTime)
            {
                Deactive();
                return;
            }
            else
            {
                float currentTime = Time.time;

                if (currentTime - lastTimeDealDamage > timeApplyDamage)
                {
                    lastTimeDealDamage = currentTime;
                    DealDamage();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

        if (unit != null && unit.CompareTag(StaticValue.TAG_PLAYER))
        {
            if (victims.Contains(unit) == false)
                victims.Add(unit);

            AdjustSlow(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

        if (unit != null && unit.CompareTag(StaticValue.TAG_PLAYER))
        {
            if (victims.Contains(unit))
            {
                AdjustSlow(false);
                victims.Remove(unit);
            }

        }
    }

    public void Active(Vector2 position)
    {
        transform.position = position;
        timeOut = 0;
        isActive = true;

        gameObject.SetActive(true);
    }

    public void Deactive()
    {
        AdjustSlow(false);
        victims.Clear();

        isActive = false;
        gameObject.SetActive(false);

        PoolingController.Instance.poolPoisonTrap.Store(this);
    }

    private void DealDamage()
    {
        for (int i = 0; i < victims.Count; i++)
        {
            victims[i].TakeDamage(damage);
        }
    }

    private void AdjustSlow(bool isSlow)
    {
        for (int i = 0; i < victims.Count; i++)
        {
            if (isSlow)
            {
                //victims[i].AddModifier(new ModifierPercent(-slowPercent, StatsType.MoveSpeed));
                victims[i].AddModifier(new ModifierData(StatsType.MoveSpeed, ModifierType.AddPercentBase, -slowPercent));
            }
            else
            {
                //victims[i].RemoveModifier(StatsType.MoveSpeed, -slowPercent);
                victims[i].RemoveModifier(new ModifierData(StatsType.MoveSpeed, ModifierType.AddPercentBase, -slowPercent));
            }

            victims[i].ReloadStats();
        }
    }
}
