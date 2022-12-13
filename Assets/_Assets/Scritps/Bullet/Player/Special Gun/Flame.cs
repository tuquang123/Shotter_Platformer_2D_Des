using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flame : MonoBehaviour
{
    public GunFlame gun;
    public AudioClip soundFire;
    public AudioSource aud;
    public Collider2D col;
    public GameObject subFlame1;
    public GameObject subFlame2;

    private bool isActive;
    private List<BaseUnit> victims = new List<BaseUnit>();
    private float timeApplyDamage = 0.3f;
    private float timerDealDamage;

    protected void Awake()
    {
        aud.loop = true;
        aud.clip = soundFire;

        timeApplyDamage = ((SO_GunFlameStats)gun.baseStats).TimeApplyDamage;
    }

    protected void Update()
    {
        if (isActive)
        {
            timerDealDamage += Time.deltaTime;

            if (timerDealDamage >= timeApplyDamage)
            {
                timerDealDamage = 0;
                DealDamage();
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
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

        if (unit != null && victims.Contains(unit) == false)
        {
            victims.Add(unit);
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
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

        if (unit != null && victims.Contains(unit))
        {
            victims.Remove(unit);
        }
    }

    public void Active()
    {
        gameObject.SetActive(true);
        isActive = true;
        aud.Play();
        timerDealDamage = 0;
    }

    public void Deactive()
    {
        victims.Clear();

        isActive = false;
        aud.Stop();
        gameObject.SetActive(false);
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

    private void DealDamage()
    {
        if (victims.Count <= 0)
            return;

        for (int i = 0; i < victims.Count; i++)
        {
            AttackData atkData = ((Rambo)gun.shooter).GetCurentAttackData();
            victims[i].TakeDamage(atkData);
        }

        gun.ConsumeAmmo();
    }
}
