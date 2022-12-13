#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_GrenadeStats : ScriptableObject
{
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _radius;
    [SerializeField]
    private float _cooldown;
    [SerializeField]
    private float _criticalRate;
    [SerializeField]
    private float _criticalDamageBonus;


    public float Damage { get { return _damage; } }
    public float Radius { get { return _radius; } }
    public float Cooldown { get { return _cooldown; } }
    public float CriticalRate { get { return _criticalRate; } }
    public float CriticalDamageBonus { get { return _criticalDamageBonus; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Grenade Stats")]
    public static void CreateMyAsset()
    {
        SO_GrenadeStats asset = ScriptableObject.CreateInstance<SO_GrenadeStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Weapon/GrenadeStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
