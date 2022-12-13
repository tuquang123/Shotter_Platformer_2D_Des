#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_GunRocketChaserStats : SO_GunStats
{

    [SerializeField]
    private float _radiusDealDamage;

    public float RadiusDealDamage { get { return _radiusDealDamage; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Gun Rocket Chaser Stats")]
    public static new void CreateMyAsset()
    {
        SO_GunRocketChaserStats asset = ScriptableObject.CreateInstance<SO_GunRocketChaserStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Weapon/gun_rocket_chaser_lv1.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
