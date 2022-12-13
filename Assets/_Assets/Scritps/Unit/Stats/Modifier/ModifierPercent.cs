public class ModifierPercent : BaseModifier
{
    public override int Priority
    {
        get { return 0; }
    }

    public ModifierPercent(float value, StatsType stats)
        : base(value, stats)
    {
        this.Type = ModifierType.AddPercentBase;
    }

    public override float GetModStatsValue(float stats, float baseStats)
    {
        return baseStats * Value;
    }
}