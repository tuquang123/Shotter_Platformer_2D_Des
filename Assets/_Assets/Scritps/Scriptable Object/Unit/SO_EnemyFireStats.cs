#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_EnemyFireStats : SO_BaseUnitStats
{
    [SerializeField]
    private float _timeApplyDamage;
    [SerializeField]
    private float _slowPercent;


    public float TimeApplyDamage { get { return _timeApplyDamage; } }
    public float SlowPercent { get { return _slowPercent; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Enemy Fire Stats")]
    public new static void CreateMyAsset()
    {
        SO_EnemyFireStats asset = ScriptableObject.CreateInstance<SO_EnemyFireStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Unit/EnemyFireStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
