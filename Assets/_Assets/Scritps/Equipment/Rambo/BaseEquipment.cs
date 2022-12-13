using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEquipment : MonoBehaviour
{
    public string equipmentName;
    public int id;
    public int level;

    protected bool isLoadedScriptableObject;

    [Header("OPTION STATS")]
    [SerializeField]
    protected List<EquipmentOption> options = new List<EquipmentOption>();


    public abstract void Init(int level);

    public abstract void ApplyOptions(BaseUnit unit);

    public abstract void LoadScriptableObject();
}
