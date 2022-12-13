using UnityEngine;
using System.Collections;

public class GunDropData
{
    public int gunId;
    public int level;
    public int ammo;

    public GunDropData(int gunId, int level, int ammo)
    {
        this.gunId = gunId;
        this.level = level;
        this.ammo = ammo;
    }
}
