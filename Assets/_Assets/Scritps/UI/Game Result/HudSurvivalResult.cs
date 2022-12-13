using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudSurvivalResult : MonoBehaviour
{
    public Text soldierKill;
    public Text vehicleKill;
    public Text bossKill;
    public Text comboKill;

    public Text soldierScore;
    public Text vehicleScore;
    public Text bossScore;
    public Text comboKillScore;
    public Text timeScore;

    public Text totalScore;
    public Text seasonScore;
    public Text coinReward;

    public void Open(SurvivalResultData data)
    {
        soldierKill.text = data.soldierKill.ToString("n0");
        vehicleKill.text = data.vehicleKill.ToString("n0");
        bossKill.text = data.bossKill.ToString("n0");
        comboKill.text = data.highestComboKill.ToString("n0");

        soldierScore.text = data.soldierScore.ToString("n0");
        vehicleScore.text = data.vehicleScore.ToString("n0");
        bossScore.text = data.bossScore.ToString("n0");
        comboKillScore.text = data.highestComboKill.ToString("n0");
        timeScore.text = data.timeScore.ToString("n0");

        totalScore.text = data.totalScore.ToString("n0");
        seasonScore.text = GameData.playerTournamentData.score.ToString("n0");

        int coin = data.totalScore;
        coinReward.text = coin.ToString("n0");
        GameData.playerResources.ReceiveCoin(coin);

        gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        SoundManager.Instance.PlaySfxClick();
        UIController.Instance.BackToMainMenu();
    }
}
