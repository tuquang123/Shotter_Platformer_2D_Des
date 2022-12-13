#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_EnemyHelicopterStats : SO_EnemyHasProjectileStats
{
    [SerializeField]
    private float _projectileSpeed;
    [SerializeField]
    private int _numberOfProjectilePerShot;


    public float ProjectileSpeed { get { return _projectileSpeed; } }
    public int NumberOfProjectilePerShot { get { return _numberOfProjectilePerShot; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Enemy Helicopter Stats")]
    public new static void CreateMyAsset()
    {
        SO_EnemyHelicopterStats asset = ScriptableObject.CreateInstance<SO_EnemyHelicopterStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Unit/EnemyHelicopterStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
