using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapSurvival : MonoBehaviour
{
    [Header("MARGIN")]
    public Transform marginLeft;
    public Transform marginTop;
    public Transform marginRight;
    public Transform marginBottom;

    [Header("")]
    public Transform playerSpawnPoint;
    public Transform cameraInitialPoint;
    public BaseSpawnLocation[] locations;
    //public Transform[] itemSpawnPoints;
    //public SO_SurvivalWave[] wavesInfo;
    //public BaseEnemy[] enemyPrefabs;
    //public TriggerPointBoss[] pointBosses;

    public void Init()
    {
        SetLocationId();
        SetDefaultMapMargin();
    }

    public void SetDefaultMapMargin()
    {
        CameraFollow.Instance.SetMarginTop(marginTop.position.y);
        CameraFollow.Instance.SetMarginLeft(marginLeft.position.x);
        CameraFollow.Instance.SetMarginRight(marginRight.position.x);
        CameraFollow.Instance.SetMarginBottom(marginBottom.position.y);
    }

    private void SetLocationId()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            locations[i].id = i;
        }
    }

    public List<int> GetLocationCanSpawnUnit(SurvivalEnemy unit)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < locations.Length; i++)
        {
            if (locations[i].noSpawnTypes.Contains(unit) == false && locations[i].isSpawning == false)
            {
                result.Add(locations[i].id);
            }
        }

        return result;
    }

    public void AddUnitToSpawnLocation(SurvivalEnemy enemy, int locationId, int minLevelUnit, int maxLevelUnit)
    {
        for (int i = 0; i < locations.Length; i++)
        {
            if (locations[i].id == locationId)
            {
                locations[i].AddUnit(enemy, minLevelUnit, maxLevelUnit);

            }
        }
    }

    public void Spawn()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            BaseSpawnLocation location = locations[i];

            if (location.CanSpawn())
            {
                location.Spawn();
            }
        }
    }

    //public void HideBossPoints()
    //{
    //    for (int i = 0; i < pointBosses.Length; i++)
    //    {
    //        pointBosses[i].gameObject.SetActive(false);
    //    }
    //}
}
