#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_EnemySniperStats : SO_BaseUnitStats
{
    [SerializeField]
    private float _knifeDamage;
    [SerializeField]
    private float _knifeAttackTimePerSecond;

    public float KnifeDamage { get { return _knifeDamage; } }
    public float KnifeAttackTimePerSecond { get { return _knifeAttackTimePerSecond; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Enemy Sniper Stats")]
    public new static void CreateMyAsset()
    {
        SO_EnemySniperStats asset = ScriptableObject.CreateInstance<SO_EnemySniperStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Unit/EnemySniperStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
