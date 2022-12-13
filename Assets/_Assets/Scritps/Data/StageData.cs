using UnityEngine;
using System.Collections;

public class StageData
{
    public string id;
    public Difficulty difficulty;

    public StageData(string stageNameId, Difficulty difficulty)
    {
        this.id = stageNameId;
        this.difficulty = difficulty;
    }
}
