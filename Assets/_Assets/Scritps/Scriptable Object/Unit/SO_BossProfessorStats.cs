#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_BossProfessorStats : SO_BaseUnitStats
{
    [SerializeField]
    private float _satelliteHp;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _shootDuration;
    [SerializeField]
    private float _energyPulseDamage;
    [SerializeField]
    private int _numberEnemySpawn;
    [SerializeField]
    private int _enemyMinLevel;
    [SerializeField]
    private int _enemyMaxLevel;


    public float SatelliteHp { get { return _satelliteHp; } }
    public float RotateSpeed { get { return _rotateSpeed; } }
    public float ShootDuration { get { return _shootDuration; } }
    public float EnergyPulseDamage { get { return _energyPulseDamage; } }
    public float NumberEnemySpawn { get { return _numberEnemySpawn; } }
    public int EnemyMinLevel { get { return _enemyMinLevel; } }
    public int EnemyMaxLevel { get { return _enemyMaxLevel; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Boss Professor Stats")]
    public new static void CreateMyAsset()
    {
        SO_BossProfessorStats asset = ScriptableObject.CreateInstance<SO_BossProfessorStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Unit/BossProfessorStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
