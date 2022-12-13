#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_BossMegatankStats : SO_BaseUnitStats
{
    [SerializeField]
    private float _plasmaDuration;
    [SerializeField]
    private float _rocketDamage;
    [SerializeField]
    private float _rocketRadius;
    [SerializeField]
    private float _goreDamage;
    [SerializeField]
    private float _rageGunDamage;
    [SerializeField]
    private float _rageRocketDamage;
    [SerializeField]
    private float _rageGoreDamage;
    [SerializeField]
    private float _rageAttackTimeSecond;
    [SerializeField]
    private float _rageBulletSpeed;


    public float PlasmaDuration { get { return _plasmaDuration; } }
    public float RocketDamage { get { return _rocketDamage; } }
    public float RocketRadius { get { return _rocketRadius; } }
    public float GoreDamage { get { return _goreDamage; } }
    public float RageGunDamage { get { return _rageGunDamage; } }
    public float RageRocketDamage { get { return _rageRocketDamage; } }
    public float RageGoreDamage { get { return _rageGoreDamage; } }
    public float RageAttackTimeSecond { get { return _rageAttackTimeSecond; } }
    public float RageBulletSpeed { get { return _rageBulletSpeed; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Boss Megatank Stats")]
    public new static void CreateMyAsset()
    {
        SO_BossMegatankStats asset = ScriptableObject.CreateInstance<SO_BossMegatankStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Unit/BossMegatankStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
