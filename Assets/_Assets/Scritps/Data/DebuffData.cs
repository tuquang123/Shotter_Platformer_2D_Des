using UnityEngine;
using System.Collections;

public class DebuffData
{
    public DebuffType type;
    public float duration;
    public float damage;

    public DebuffData(DebuffType type, float duration, float damage = 0)
    {
        this.type = type;
        this.duration = duration;
        this.damage = damage;
    }
}
