using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnData
{
    public int index;
    public int id;
    public int level;
    public Vector2 position;
    public int zoneId;
    public int packId;
    public bool isMainUnit;
    public bool isCanMove;
    public bool isCanJump;
    public bool isRunPassArea;
    public int bounty;
    public List<ItemDropData> items;

    public EnemySpawnData() { }

    public EnemySpawnData(int id, int level, Vector2 position, int zoneId, int packId,
        bool isMainUnit = true, bool isCanMove = true, bool isCanJump = false, bool isRunPassArea = false, List<ItemDropData> items = null)
    {
        this.id = id;
        this.level = level;
        this.position = position;
        this.zoneId = zoneId;
        this.packId = packId;
        this.isMainUnit = isMainUnit;
        this.isCanMove = isCanMove;
        this.isCanJump = isCanJump;
        this.isRunPassArea = isRunPassArea;
        this.items = items;
    }
}
