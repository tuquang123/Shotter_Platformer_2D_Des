#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_GunLaserStats : SO_GunStats
{
    [SerializeField]
    private float _timeApplyDamage;

    public float TimeApplyDamage { get { return _timeApplyDamage; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Gun Laser Stats")]
    public static new void CreateMyAsset()
    {
        SO_GunLaserStats asset = ScriptableObject.CreateInstance<SO_GunLaserStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Weapon/gun_laser_lv1.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
