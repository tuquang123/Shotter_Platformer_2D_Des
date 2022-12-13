#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_BossSubmarineStats : SO_BaseUnitStats
{
    [SerializeField]
    private int _numberOfBullet;
    [SerializeField]
    private int _numberOfRocket;
    [SerializeField]
    private float _timeDelayRocket;
    [SerializeField]
    private float _rocketSpeed;
    [SerializeField]
    private float _rocketDamage;
    [SerializeField]
    private int _numberOfMarine;
    [SerializeField]
    private float _timeDelaySpawnMarine;
    [SerializeField]
    private int _marineLevel;
    [SerializeField]
    private float _goreDamage;
    [SerializeField]
    private int _rageNumberOfRocket;
    [SerializeField]
    private float _rageRocketDamage;
    [SerializeField]
    private float _rageRocketSpeed;
    [SerializeField]
    private int _rageNumberOfMarine;
    [SerializeField]
    private int _rageMarineLevel;
    [SerializeField]
    private float _rageGoreDamage;



    public int NumberOfBullet { get { return _numberOfBullet; } }
    public int NumberOfRocket { get { return _numberOfRocket; } }
    public float TimeDelayRocket { get { return _timeDelayRocket; } }
    public float RocketSpeed { get { return _rocketSpeed; } }
    public float RocketDamage { get { return _rocketDamage; } }
    public int NumberOfMarine { get { return _numberOfMarine; } }
    public float TimeDelaySpawnMarine { get { return _timeDelaySpawnMarine; } }
    public int MarineLevel { get { return _marineLevel; } }
    public float GoreDamage { get { return _goreDamage; } }
    public int RageNumberOfRocket { get { return _rageNumberOfRocket; } }
    public float RageRocketDamage { get { return _rageRocketDamage; } }
    public float RageRocketSpeed { get { return _rageRocketSpeed; } }
    public int RageNumberOfMarine { get { return _rageNumberOfMarine; } }
    public int RageMarineLevel { get { return _rageMarineLevel; } }
    public float RageGoreDamage { get { return _rageGoreDamage; } }



#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Boss Submarine Stats")]
    public new static void CreateMyAsset()
    {
        SO_BossSubmarineStats asset = ScriptableObject.CreateInstance<SO_BossSubmarineStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Unit/BossSubmarineStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
