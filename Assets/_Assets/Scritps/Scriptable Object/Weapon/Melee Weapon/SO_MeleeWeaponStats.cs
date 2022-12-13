#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_MeleeWeaponStats : ScriptableObject
{
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _attackTimePerSecond;
    [SerializeField]
    private float _criticalRate;
    [SerializeField]
    private float _criticalDamageBonus;


    public float Damage { get { return _damage; } }
    public float AttackTimePerSecond { get { return _attackTimePerSecond; } }
    public float CriticalRate { get { return _criticalRate; } }
    public float CriticalDamageBonus { get { return _criticalDamageBonus; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Melee Weapon Stats")]
    public static void CreateMyAsset()
    {
        SO_MeleeWeaponStats asset = ScriptableObject.CreateInstance<SO_MeleeWeaponStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Weapon/MeleeWeaponStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
