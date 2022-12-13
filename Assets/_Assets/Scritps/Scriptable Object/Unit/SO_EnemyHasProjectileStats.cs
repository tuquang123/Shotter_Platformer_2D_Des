#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_EnemyHasProjectileStats : SO_BaseUnitStats
{
    [SerializeField]
    private float _projectileDamage;
    [SerializeField]
    private float _projectileDamageRadius;


    public float ProjectileDamage { get { return _projectileDamage; } }
    public float ProjectileDamageRadius { get { return _projectileDamageRadius; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Enemy Has Projectile Stats")]
    public new static void CreateMyAsset()
    {
        SO_EnemyHasProjectileStats asset = ScriptableObject.CreateInstance<SO_EnemyHasProjectileStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Unit/EnemyHasProjectileStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
