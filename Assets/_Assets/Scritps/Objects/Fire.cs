using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour
{
    public bool isActive;
    public AudioClip soundFire;
    public Transform fireEffect;

    private AudioSource aud;
    private EnemyFire owner;
    private BoxCollider2D col;
    private List<BaseUnit> victims = new List<BaseUnit>();

    private float timeApplyDamage = 0.3f;
    private float slowPercent = 0.1f;

    private float timeOut;
    private float lastTimeDealDamage;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
        aud.loop = true;
        aud.clip = soundFire;

        owner = transform.root.GetComponent<EnemyFire>();
        col = GetComponent<BoxCollider2D>();

        timeApplyDamage = ((SO_EnemyFireStats)owner.baseStats).TimeApplyDamage;
        slowPercent = Mathf.Clamp01(((SO_EnemyFireStats)owner.baseStats).SlowPercent / 100f);
    }

    private void Update()
    {
        if (isActive)
        {
            float currentTime = Time.time;
            float deltaTime = currentTime - lastTimeDealDamage;
            if (deltaTime > timeApplyDamage)
            {
                lastTimeDealDamage = currentTime;
                DealDamage();
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

    public void Active()
    {
        gameObject.SetActive(true);
        timeOut = 0;
        isActive = true;
        aud.Play();
    }

    public void Deactive()
    {
        AdjustSlow(false);
        victims.Clear();

        isActive = false;
        aud.Stop();
        gameObject.SetActive(false);
    }

    private void DealDamage()
    {
        for (int i = 0; i < victims.Count; i++)
        {
            AttackData atkData = new AttackData(owner, owner.baseStats.Damage);
            victims[i].TakeDamage(atkData);
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
