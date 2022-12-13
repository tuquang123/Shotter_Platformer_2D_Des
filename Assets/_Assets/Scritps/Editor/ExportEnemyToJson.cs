using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using Newtonsoft.Json;
#endif

public class ExportEnemyToJson : MonoBehaviour
{
#if UNITY_EDITOR

    [MenuItem("Tools/Save Enemy Data")]

    public static void ExportEmemyData()
    {
        Map map = GameObject.FindObjectOfType<Map>();

        if (map.gameObject.activeInHierarchy)
        {
            string mapName = map.transform.root.name.Split('-').Last();

            MapData mapData = new MapData();

            #region Grounded enemies
            List<EnemySpawnData> enemyData = new List<EnemySpawnData>();
            BaseEnemy[] enemies = GameObject.FindObjectsOfType<BaseEnemy>().Where(x => x.gameObject.activeInHierarchy).ToArray();

            if (enemies.Length <= 0)
            {
                DebugCustom.Log("No enemies");
            }
            else
            {
                DebugCustom.Log(string.Format("Map {0} has {1} grounded enemies", mapName, enemies.Length));
                Dictionary<int, int> dictEnemies = new Dictionary<int, int>();

                for (int i = 0; i < enemies.Length; i++)
                {
                    BaseEnemy enemy = enemies[i];

                    EnemySpawnData data = new EnemySpawnData();
                    data.index = i;
                    data.id = enemy.id;
                    data.packId = int.Parse(enemy.transform.parent.name);
                    data.level = enemy.level;
                    data.isCanMove = enemy.canMove;
                    data.isCanJump = enemy.canJump;
                    data.isMainUnit = enemy.isMainUnit;
                    data.isRunPassArea = enemy.isRunPassArea;
                    data.position = enemy.transform.position;
                    data.items = enemy.itemDropList;

                    string zoneName = enemy.transform.parent.parent.name;
                    data.zoneId = int.Parse(zoneName.Split('-').Last());

                    enemyData.Add(data);

                    if (dictEnemies.ContainsKey(data.id))
                    {
                        int count = dictEnemies[data.id];
                        count++;
                        dictEnemies[data.id] = count;
                    }
                    else
                    {
                        dictEnemies.Add(data.id, 1);
                    }
                }

                mapData.enemyData = enemyData;
                DebugCustom.Log(string.Format("EnemyCount={0}", JsonConvert.SerializeObject(dictEnemies)));
            }
            #endregion

            #region Bombers
            List<BomberPointData> bomberData = new List<BomberPointData>();
            TriggerPointBomber[] bomberPoints = GameObject.FindObjectsOfType<TriggerPointBomber>();

            if (bomberPoints.Length <= 0)
            {
                DebugCustom.Log("No bombers");
            }
            else
            {
                DebugCustom.Log(string.Format("Map {0} has {1} bombers", mapName, bomberPoints.Length));

                for (int j = 0; j < bomberPoints.Length; j++)
                {
                    TriggerPointBomber point = bomberPoints[j];

                    BomberPointData data = new BomberPointData(point.transform.position, point.isFromLeft, point.levelInNormal);
                    bomberData.Add(data);
                }

                mapData.bomberData = bomberData;
            }
            #endregion

            #region Helicopters
            List<HelicopterPointData> helicopterData = new List<HelicopterPointData>();
            TriggerPointHelicopter[] helicopterPoints = GameObject.FindObjectsOfType<TriggerPointHelicopter>();

            if (helicopterPoints.Length <= 0)
            {
                DebugCustom.Log("No helicopter");
            }
            else
            {
                DebugCustom.Log(string.Format("Map {0} has {1} helicopters", mapName, helicopterPoints.Length));

                for (int j = 0; j < helicopterPoints.Length; j++)
                {
                    TriggerPointHelicopter point = helicopterPoints[j];

                    HelicopterPointData data = new HelicopterPointData(point.transform.position, point.levelInNormal, point.isFinalBoss);

                    helicopterData.Add(data);
                }

                mapData.helicopterData = helicopterData;
            }
            #endregion

            #region Boss
            TriggerPointBoss bossPoint = GameObject.FindObjectOfType<TriggerPointBoss>();

            if (bossPoint == null)
            {
                DebugCustom.Log("No Boss");
            }
            else
            {
                DebugCustom.Log(string.Format("Map {0} has boss id={1}", mapName, bossPoint.bossPrefab.id));

                BossPointData bossData = new BossPointData(bossPoint.transform.position, bossPoint.bossPrefab.id);

                mapData.bossData = bossData;
            }
            #endregion

            string path = string.Format("Assets/_Assets/Resources/JSON/Map Enemy Data/{0}.json", mapName);
            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(JsonConvert.SerializeObject(mapData));
            writer.Close();

            DebugCustom.Log(string.Format("Save to {0}", path));

            AssetDatabase.ImportAsset(path);
        }
    }
#endif
}
