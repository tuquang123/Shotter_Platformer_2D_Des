#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_BossVenomStats : SO_BaseUnitStats
{
    [SerializeField]
    private int _shootTimes;
    [SerializeField]
    private float _delayShootTime;
    [SerializeField]
    private float _laserDamage;
    [SerializeField]
    private float _rageLaserDamage;
    [SerializeField]
    private int _rageShootTimes;
    [SerializeField]
    private float _rageDamage;
    [SerializeField]
    private float _rageBulletSpeed;


    public int ShootTimes { get { return _shootTimes; } }
    public float DelayShootTime { get { return _delayShootTime; } }
    public float LaserDamage { get { return _laserDamage; } }
    public float RageLaserDamage { get { return _rageLaserDamage; } }
    public int RageShootTimes { get { return _rageShootTimes; } }
    public float RageDamage { get { return _rageDamage; } }
    public float RageBulletSpeed { get { return _rageBulletSpeed; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Boss Venom Stats")]
    public new static void CreateMyAsset()
    {
        SO_BossVenomStats asset = ScriptableObject.CreateInstance<SO_BossVenomStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Unit/BossVenomStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
