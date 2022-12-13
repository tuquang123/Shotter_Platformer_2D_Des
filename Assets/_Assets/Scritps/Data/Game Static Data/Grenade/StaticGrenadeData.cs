using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticGrenadeData
{
    public int id;
    public string grenadeName;
    public string statsPath;

    public int coinUnlock;
    public int gemUnlock;
    public int medalUnlock;
    public int pricePerUnit;

    public int[] upgradeInfo;
    public List<WayToObtain> otherWayObtain;
}
