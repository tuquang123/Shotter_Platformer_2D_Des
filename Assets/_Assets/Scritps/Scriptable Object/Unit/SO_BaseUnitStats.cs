#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_BaseUnitStats : ScriptableObject
{
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _bulletSpeed;
    [SerializeField]
    private float _hp;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _attackTimePerSecond;
    [SerializeField]
    private float _criticalRate;
    [SerializeField]
    private float _criticalDamageBonus;


    public float Damage { get { return _damage; } }
    public float HP { get { return _hp; } }
    public float MoveSpeed { get { return _moveSpeed; } }
    public float AttackTimePerSecond { get { return _attackTimePerSecond; } }
    public float BulletSpeed { get { return _bulletSpeed; } }
    public float CriticalRate { get { return _criticalRate; } }
    public float CriticalDamageBonus { get { return _criticalDamageBonus; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Base Unit Stats")]
    public static void CreateMyAsset()
    {
        SO_BaseUnitStats asset = ScriptableObject.CreateInstance<SO_BaseUnitStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Unit/BaseUnitStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
