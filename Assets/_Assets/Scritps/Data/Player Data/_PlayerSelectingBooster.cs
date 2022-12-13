using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class _PlayerSelectingBooster : List<BoosterType>
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerSelectingBooster.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerSelectingBooster=" + s);
    }
}
