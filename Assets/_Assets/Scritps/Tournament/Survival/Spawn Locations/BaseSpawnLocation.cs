using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseSpawnLocation : MonoBehaviour
{
    public int id;
    public int minLevelUnit = 1;
    public int maxLevelUnit = 10;
    public bool isSpawning;
    public Transform spawnPoint;
    public List<SurvivalEnemy> noSpawnTypes;

    protected List<SurvivalEnemy> spawnUnits = new List<SurvivalEnemy>();
    protected IEnumerator coroutineSpawn;
    protected WaitForSeconds delaySpawn = new WaitForSeconds(0.75f);


    public void AddUnit(SurvivalEnemy unit, int minLevelUnit, int maxLevelUnit)
    {
        spawnUnits.Add(unit);

        this.minLevelUnit = minLevelUnit;
        this.maxLevelUnit = maxLevelUnit;
    }

    public void Clear()
    {
        spawnUnits.Clear();
    }

    public bool CanSpawn()
    {
        return isSpawning == false && spawnUnits.Count > 0;
    }

    public abstract void Spawn();
}
