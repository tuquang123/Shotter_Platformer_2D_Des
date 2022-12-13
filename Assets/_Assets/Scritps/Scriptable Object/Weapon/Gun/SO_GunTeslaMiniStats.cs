#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_GunTeslaMiniStats : SO_GunStats
{
    [SerializeField]
    private float _stunChance;
    [SerializeField]
    private float _stunDuration;

    public float StunChance { get { return _stunChance; } }
    public float StunDuration { get { return _stunDuration; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Gun Tesla Mini Stats")]
    public static new void CreateMyAsset()
    {
        SO_GunTeslaMiniStats asset = ScriptableObject.CreateInstance<SO_GunTeslaMiniStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Weapon/gun_tesla_mini_lv1.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
