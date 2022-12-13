
public class ModifierValue : BaseModifier
{
    public override int Priority
    {
        get { return 1; }
    }

    public ModifierValue(float value, StatsType stats)
        : base(value, stats)
    {
        this.Type = ModifierType.AddPoint;
    }

    public override float GetModStatsValue(float stats, float baseStats)
    {
        return Value;
    }
}