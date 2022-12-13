#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_GunKamePowerStats : SO_GunStats
{
    [SerializeField]
    private float _chargeTime;

    public float ChargeTime { get { return _chargeTime; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Gun Kame Power Stats")]
    public static new void CreateMyAsset()
    {
        SO_GunKamePowerStats asset = ScriptableObject.CreateInstance<SO_GunKamePowerStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Weapon/gun_kame_power_lv1.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
