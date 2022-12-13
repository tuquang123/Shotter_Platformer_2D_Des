#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_GunFireBallStats : SO_GunStats
{
    [SerializeField]
    private float _timeApplyDamage;
    [SerializeField]
    private float _distance;

    public float TimeApplyDamage { get { return _timeApplyDamage; } }
    public float Distance { get { return _distance; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Gun Fire Ball Stats")]
    public static new void CreateMyAsset()
    {
        SO_GunFireBallStats asset = ScriptableObject.CreateInstance<SO_GunFireBallStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Weapon/gun_fire_ball_lv1.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
