using UnityEngine;
using System.Collections;

public class HelicopterPointData
{
    public Vector2 position;
    public int levelInNormal;
    public bool isFinalBoss;

    public HelicopterPointData(Vector2 position, int levelInNormal, bool isFinalBoss)
    {
        this.position = position;
        this.levelInNormal = levelInNormal;
        this.isFinalBoss = isFinalBoss;
    }
}
