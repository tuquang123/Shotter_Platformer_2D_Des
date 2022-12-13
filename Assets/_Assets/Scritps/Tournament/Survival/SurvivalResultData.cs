using UnityEngine;
using System.Collections;

public class SurvivalResultData
{
    public int numberWave;
    public int soldierKill;
    public int soldierScore;
    public int vehicleKill;
    public int vehicleScore;
    public int bossKill;
    public int bossScore;
    public int timeScore;
    public int highestComboKill;
    public int totalScore;

    public SurvivalResultData(int numberWave, int soldierKill, int soldierScore, int vehicleKill, int vehicleScore,
        int bossKill, int bossScore, int timeScore, int highestComboKill, int totalScore)
    {
        this.numberWave = numberWave;
        this.soldierKill = soldierKill;
        this.soldierScore = soldierScore;
        this.vehicleKill = vehicleKill;
        this.vehicleScore = vehicleScore;
        this.bossKill = bossKill;
        this.bossScore = bossScore;
        this.timeScore = timeScore;
        this.highestComboKill = highestComboKill;
        this.totalScore = totalScore;
    }
}
