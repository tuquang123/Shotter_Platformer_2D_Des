using UnityEngine;
using System.Collections;

public class ModifierData
{
    public StatsType stats { get; private set; }
    public ModifierType type { get; private set; }
    public float value { get; set; }

    public ModifierData(StatsType stats, ModifierType type, float value)
    {
        this.stats = stats;
        this.type = type;
        this.value = value;
    }
}
