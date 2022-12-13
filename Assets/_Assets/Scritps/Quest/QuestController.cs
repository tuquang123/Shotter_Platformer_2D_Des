using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public List<bool> result;
    public BaseQuest[] listQuest;


    public void Init()
    {
        result = new List<bool> { false, false, false };

        for (int i = 0; i < listQuest.Length; i++)
        {
            listQuest[i].Init();
        }
    }

    public string GetDescription(int index)
    {
        if (index >= listQuest.Length)
            return string.Empty;

        string des = listQuest[index].GetDescription();

        return des;
    }

    public string GetProgress(int index)
    {
        if (index >= listQuest.Length)
            return string.Empty;

        string progress = listQuest[index].GetCurrentProgress();

        return progress;
    }

    public bool IsAlreadyCompleted(int index)
    {
        if (index >= listQuest.Length)
            return false;

        return listQuest[index].IsCompleted();
    }

    public void LoadQuestProgressWhenWin()
    {
        result[0] = true;
        result[1] = listQuest[1].IsCompleted();
        result[2] = listQuest[2].IsCompleted();
    }

    public int GetStar()
    {
        int star = 0;

        for (int i = 0; i < result.Count; i++)
        {
            if (result[i] == true)
                star++;
        }

        return star;
    }
}
