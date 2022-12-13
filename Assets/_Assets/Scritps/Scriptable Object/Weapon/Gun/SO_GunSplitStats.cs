#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_GunSplitStats : SO_GunStats
{
    [SerializeField]
    private float _damageSplit;

    public float DamageSplit { get { return _damageSplit; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Gun Split Stats")]
    public static new void CreateMyAsset()
    {
        SO_GunSplitStats asset = ScriptableObject.CreateInstance<SO_GunSplitStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Weapon/gun_split_lv1.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
