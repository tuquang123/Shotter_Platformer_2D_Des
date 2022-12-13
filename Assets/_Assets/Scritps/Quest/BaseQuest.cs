using UnityEngine;
using System.Collections;

public class BaseQuest : MonoBehaviour
{
    protected bool isCompleted;
    protected string keyDescription;
    protected string description;

    public virtual void Init()
    {
        if (GameData.questDescriptions.ContainsKey(keyDescription))
        {
            description = GameData.questDescriptions[keyDescription];
        }
        else
        {
            DebugCustom.LogError("[Quest description] Key not found=" + keyDescription);
            description = string.Empty;
        }
    }

    public void SetComplete(bool isCompleted)
    {
        this.isCompleted = isCompleted;
    }

    public virtual string GetDescription()
    {
        return description;
    }

    public virtual string GetCurrentProgress()
    {
        return string.Empty;
    }

    public virtual bool IsCompleted()
    {
        return isCompleted;
    }
}
