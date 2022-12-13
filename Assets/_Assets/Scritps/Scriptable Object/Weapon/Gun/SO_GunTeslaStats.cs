#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_GunTeslaStats : SO_GunStats
{
    [SerializeField]
    private float _timeApplyDamage;
    [SerializeField]
    private int _numberEnemyChain;

    public float TimeApplyDamage { get { return _timeApplyDamage; } }
    public int NumberEnemyChain { get { return _numberEnemyChain; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Gun Tesla Stats")]
    public static new void CreateMyAsset()
    {
        SO_GunTeslaStats asset = ScriptableObject.CreateInstance<SO_GunTeslaStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Weapon/gun_tesla_lv1.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
