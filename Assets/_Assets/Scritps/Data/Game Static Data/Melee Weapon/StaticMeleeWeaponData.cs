using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticMeleeWeaponData
{
    public int id;
    public string weaponName;
    public string statsPath;

    public int coinUnlock;
    public int gemUnlock;
    public int medalUnlock;

    public int[] upgradeInfo;
    public List<WayToObtain> otherWayObtain;
}
