using UnityEngine;
using System.Collections;

public class BomberPointData
{
    public Vector2 position;
    public bool isFromLeft;
    public int levelInNormal;

    public BomberPointData(Vector2 position, bool isFromLeft, int levelInNormal)
    {
        this.position = position;
        this.isFromLeft = isFromLeft;
        this.levelInNormal = levelInNormal;
    }
}
