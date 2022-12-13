using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class _PlayerTutorialData : Dictionary<TutorialType, bool>
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerTutorialData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerTutorial=" + s);
    }

    public bool IsCompletedStep(TutorialType type)
    {
        if (ContainsKey(type))
        {
            return this[type];
        }
        else
        {
            Add(type, false);
            return false;
        }
    }

    public void SetComplete(TutorialType type)
    {
        if (ContainsKey(type))
        {
            this[type] = true;
        }
        else
        {
            Add(type, true);
        }

        Save();
    }

    public void SkipTutorial(TutorialType type)
    {
        SetComplete(type);

        Save();
    }
}
