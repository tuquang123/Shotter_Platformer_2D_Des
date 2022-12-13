using UnityEngine;
using System.Collections;

public class PlayerGrenadeData
{
    public int id;
    public int level;
    public int quantity;
    public bool isNew;

    public PlayerGrenadeData(int id, int level = 1, int quantity = 1)
    {
        this.id = id;
        this.level = level;
        this.quantity = quantity;
    }
}
