using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackData
{
    public BaseUnit attacker;
    public WeaponType weapon;
    public List<DebuffData> debuffs;
    public float damage;
    public float radiusDealDamage;
    public bool isCritical;
    public int weaponId;

    public AttackData(BaseUnit attacker, float damage,
        float radiusDealDamage = 0f, bool isCritical = false, WeaponType weapon = WeaponType.NormalGun, int weaponId = -1, List<DebuffData> debuffs = null)
    {
        this.attacker = attacker;
        this.damage = damage;
        this.debuffs = debuffs;
        this.radiusDealDamage = radiusDealDamage;
        this.isCritical = isCritical;
        this.weapon = weapon;
        this.weaponId = weaponId;
    }
}
