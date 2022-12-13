using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoisonWater : MonoBehaviour
{
    public float damagePerSecond = 20f;
    public float timeApplyDamage = 0.5f;
    public float slowPercent = 0.5f;

    private BoxCollider2D col;
    private List<BaseUnit> victims = new List<BaseUnit>();

    private float lastTimeDealDamage;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();

        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, OnUnitDie);
    }

    private void Update()
    {
        if (victims.Count <= 0)
            return;

        float currentTime = Time.time;

        if (currentTime - lastTimeDealDamage > timeApplyDamage)
        {
            lastTimeDealDamage = currentTime;
            DealDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER) || other.transform.root.CompareTag(StaticValue.TAG_ENEMY))
        {
            BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

            if (unit != null && unit.isDead == false && unit.isOnVehicle == false)
            {
                if (victims.Contains(unit) == false)
                    victims.Add(unit);

                SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_TRIGGER_WATER);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER) || other.transform.root.CompareTag(StaticValue.TAG_ENEMY))
        {
            BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

            if (unit != null)
            {
                if (victims.Contains(unit))
                {
                    victims.Remove(unit);
                }
            }
        }
    }

    private void DealDamage()
    {
        for (int i = 0; i < victims.Count; i++)
        {
            victims[i].TakeDamage(damagePerSecond);
        }
    }

    //private void AdjustSlow(bool isSlow)
    //{
    //    for (int i = 0; i < victims.Count; i++)
    //    {
    //        if (isSlow)
    //        {
    //            victims[i].AddModifier(new ModifierPercent(-slowPercent, StatsType.MoveSpeed));
    //            victims[i].ApplyModifier();
    //        }
    //        else
    //        {
    //            victims[i].RemoveModifier(-slowPercent, StatsType.MoveSpeed);
    //            victims[i].ApplyModifier();
    //        }
    //    }
    //}

    private void OnUnitDie(Component senser, object param)
    {
        UnitDieData data = (UnitDieData)param;

        if (data.unit != null && victims.Contains(data.unit))
        {
            victims.Remove(data.unit);
        }
    }
}
