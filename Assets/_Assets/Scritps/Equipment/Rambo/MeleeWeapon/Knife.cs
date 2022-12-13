using UnityEngine;
using System.Collections;

public class Knife : BaseMeleeWeapon
{
    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_KNIFE, level);
        baseStats = Resources.Load<SO_MeleeWeaponStats>(path);
    }
}
