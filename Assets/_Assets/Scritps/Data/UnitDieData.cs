using UnityEngine;
using System.Collections;

public class UnitDieData
{
    public BaseUnit unit;
    public AttackData attackData;

    public UnitDieData(BaseUnit unit, AttackData attackData = null)
    {
        this.unit = unit;
        this.attackData = attackData;
    }
}
