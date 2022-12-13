#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SO_BossMonkeyStats : SO_BaseUnitStats
{
    [SerializeField]
    private float _stoneDamage;
    [SerializeField]
    private float _spikeDamage;
    [SerializeField]
    private float _spikeSpeed;
    [SerializeField]
    private float _spikeDelay;
    [SerializeField]
    private int _numberSpikes;
    [SerializeField]
    private int _numberMinions;
    [SerializeField]
    private int _levelMinions;

    [SerializeField]
    private float _hp65_stoneDamage;
    [SerializeField]
    private float _hp65_spikeDamage;
    [SerializeField]
    private float _hp65_spikeSpeed;
    [SerializeField]
    private float _hp65_spikeDelay;
    [SerializeField]
    private int _hp65_numberSpikes;
    [SerializeField]
    private int _hp65_numberMinions;

    [SerializeField]
    private float _hp35_stoneDamage;
    [SerializeField]
    private float _hp35_spikeDamage;
    [SerializeField]
    private float _hp35_spikeSpeed;
    [SerializeField]
    private float _hp35_spikeDelay;
    [SerializeField]
    private int _hp35_numberSpikes;
    [SerializeField]
    private int _hp35_numberMinions;


    public float StoneDamage { get { return _stoneDamage; } }
    public float SpikeDamage { get { return _spikeDamage; } }
    public float SpikeSpeed { get { return _spikeSpeed; } }
    public float SpikeDelay { get { return _spikeDelay; } }
    public int NumberSpikes { get { return _numberSpikes; } }
    public int NumberMinions { get { return _numberMinions; } }
    public int LevelMinions { get { return _levelMinions; } }

    public float Hp65_StoneDamage { get { return _hp65_stoneDamage; } }
    public float Hp65_SpikeDamage { get { return _hp65_spikeDamage; } }
    public float Hp65_SpikeSpeed { get { return _hp65_spikeSpeed; } }
    public float Hp65_SpikeDelay { get { return _hp65_spikeDelay; } }
    public int Hp65_NumberSpikes { get { return _hp65_numberSpikes; } }
    public int Hp65_NumberMinions { get { return _hp65_numberMinions; } }

    public float Hp35_StoneDamage { get { return _hp35_stoneDamage; } }
    public float Hp35_SpikeDamage { get { return _hp35_spikeDamage; } }
    public float Hp35_SpikeSpeed { get { return _hp35_spikeSpeed; } }
    public float Hp35_SpikeDelay { get { return _hp35_spikeDelay; } }
    public int Hp35_NumberSpikes { get { return _hp35_numberSpikes; } }
    public int Hp35_NumberMinions { get { return _hp35_numberMinions; } }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Scriptable Object/Boss Monkey Stats")]
    public new static void CreateMyAsset()
    {
        SO_BossMonkeyStats asset = ScriptableObject.CreateInstance<SO_BossMonkeyStats>();
        AssetDatabase.CreateAsset(asset, "Assets/_Assets/Data Object/Unit/BossMonkeyStats.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}
