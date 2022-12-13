using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossProfessorEnergyPulse : MonoBehaviour
{
    public Transform hitEffect;
    public LayerMask collisionMask;
    public Transform castPoint;
    public Transform destinationPoint;
    //public Transform rootBone;
    public RaycastHit2D hit;
    public List<BaseUnit> pulseVictims = new List<BaseUnit>();

    private float timerApplyDamage;
    private BossProfessor boss;

    private void Awake()
    {
        boss = transform.root.gameObject.GetComponent<BossProfessor>();
    }

    private void LateUpdate()
    {
        hit = Physics2D.Linecast(castPoint.position, destinationPoint.position, collisionMask);

        Vector2 hitPoint;

        if (hit)
        {
            hitPoint = hit.point;
        }
        else
        {
            hitPoint = destinationPoint.position;
        }

        //Vector2 v = rootBone.position;
        //v.y = hitPoint.y;
        //rootBone.position = v;

        hitEffect.transform.position = hitPoint;

        ApplyDamage();
    }

    public void Active(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private void ApplyDamage()
    {
        timerApplyDamage += Time.deltaTime;

        if (timerApplyDamage >= 0.3f)
        {
            timerApplyDamage = 0;

            for (int i = 0; i < pulseVictims.Count; i++)
            {
                float damage = ((SO_BossProfessorStats)boss.baseStats).EnergyPulseDamage;
                AttackData atkData = new AttackData(boss, damage);
                pulseVictims[i].TakeDamage(atkData);
            }
        }
    }
}
