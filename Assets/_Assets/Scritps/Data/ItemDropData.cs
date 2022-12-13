using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemDropData
{
    public ItemDropType type;
    public float value;
    public float dropRate;

    public ItemDropData(ItemDropType type, float value, float dropRate = 100f)
    {
        this.type = type;
        this.value = value;
        this.dropRate = dropRate;
    }
}
