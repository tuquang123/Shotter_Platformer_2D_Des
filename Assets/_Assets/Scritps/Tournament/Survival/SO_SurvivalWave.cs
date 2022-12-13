#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;

public class SO_SurvivalWave : ScriptableObject
{
    [SerializeField]
    private int _waveId;
    [SerializeField]
    private bool _isBossWave;
    [SerializeField]
    private BossType _bossType;
    [SerializeField]
    private int _minLevelUnit;
    [SerializeField]
    private int _maxLevelUnit;
    [SerializeField]
    private List<TimeData> _time;
    [SerializeField]
    private List<TimeDropItemData> _timeDropItem;


    public int WaveId { get { return _waveId; } }
    public bool IsBossWave { get { return _isBossWave; } }
    public BossType BossType { get { return _bossType; } }
    public int MinLevelUnit { get { return _minLevelUnit; } }
    public int MaxLevelUnit { get { return _maxLevelUnit; } }
    public List<TimeData> Time { get { return _time; } }
    public List<TimeDropItemData> TimeDropItem { get { return _timeDropItem; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Survival Wave")]
    public static void CreateMyAsset()
    {
        SO_SurvivalWave asset = ScriptableObject.CreateInstance<SO_SurvivalWave>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/SurvivalWave.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
