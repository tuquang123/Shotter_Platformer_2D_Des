using UnityEngine;
using System.Collections;

public class Guitar : BaseMeleeWeapon
{
    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUITAR, level);
        baseStats = Resources.Load<SO_MeleeWeaponStats>(path);
    }
}
