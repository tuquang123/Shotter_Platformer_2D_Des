public abstract class BaseModifier
{
    public abstract int Priority { get; }
    public float Value { get; set; }
    public ModifierType Type { get; protected set; }
    public StatsType Stats { get; protected set; }

    public BaseModifier(float value, StatsType stats)
    {
        this.Value = value;
        this.Stats = stats;
    }

    public abstract float GetModStatsValue(float stats, float baseStats);
}
