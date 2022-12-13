#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_BossMegatronStats : SO_BaseUnitStats
{
    [SerializeField]
    private float _smashDamage;
    [SerializeField]
    private float _jumpDamage;

    public float SmashDamage { get { return _smashDamage; } }
    public float JumpDamage { get { return _jumpDamage; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Boss Megatron Stats")]
    public new static void CreateMyAsset()
    {
        SO_BossMegatronStats asset = ScriptableObject.CreateInstance<SO_BossMegatronStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Unit/BossMegatronStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
