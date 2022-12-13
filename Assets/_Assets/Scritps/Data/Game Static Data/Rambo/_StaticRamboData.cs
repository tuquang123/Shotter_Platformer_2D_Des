using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _StaticRamboData : Dictionary<int, StaticRamboData>
{
    public StaticRamboData GetData(int id)
    {
        if (ContainsKey(id))
        {
            return this[id];
        }
        else
        {
            DebugCustom.LogError("[GetStaticRamboData] Key not found=" + id);
            return null;
        }
    }
}
