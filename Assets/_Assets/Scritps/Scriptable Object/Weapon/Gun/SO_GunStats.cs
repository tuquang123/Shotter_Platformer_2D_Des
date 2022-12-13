#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
#endif
using UnityEngine;

public class SO_GunStats : ScriptableObject
{
    [SerializeField]
    private bool _hasCartouche = true;
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _attackTimePerSecond;
    [SerializeField]
    private float _bulletSpeed;
    [SerializeField]
    private int _bulletPerShoot;
    [SerializeField]
    private int _ammo;
    [SerializeField]
    private float _criticalRate;
    [SerializeField]
    private float _criticalDamageBonus;


    public bool HasCartouche { get { return _hasCartouche; } }
    public float Damage { get { return _damage; } }
    public float AttackTimePerSecond { get { return _attackTimePerSecond; } }
    public float BulletSpeed { get { return _bulletSpeed; } }
    public int BulletPerShoot { get { return _bulletPerShoot; } }
    public int Ammo { get { return _ammo; } }
    public float CriticalRate { get { return _criticalRate; } }
    public float CriticalDamageBonus { get { return _criticalDamageBonus; } }



#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Gun Stats")]
    public static void CreateMyAsset()
    {
        SO_GunStats asset = ScriptableObject.CreateInstance<SO_GunStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Weapon/gun_base_lv1.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
