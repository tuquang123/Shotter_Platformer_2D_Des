using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIModeSurvival : MonoBehaviour
{
    [Header("LAYOUT")]
    public Text textWave;
    public Text textScore;
    public Text textNotice;
    public GameObject labelWaveComplete;

    [Header("SUPPORTS")]
    public GameObject groupSupportItems;
    public BaseSupportItem[] supports;


    public void Init()
    {
        textWave.text = string.Empty;
        textScore.text = string.Empty;
        HideNotice();

        groupSupportItems.SetActive(false);

        for (int i = 0; i < supports.Length; i++)
        {
            supports[i].Init();
        }
    }

    public void SetTextWave(int number)
    {
        textWave.text = number.ToString();
    }

    public void SetScoreText(int score)
    {
        textScore.text = score.ToString("n0");
    }

    public void ShowNotice(string content)
    {
        textNotice.text = content;
        textNotice.gameObject.SetActive(true);
    }

    public void HideNotice()
    {
        textNotice.text = string.Empty;
        textNotice.gameObject.SetActive(false);
    }

    public void ShowComplete()
    {
        labelWaveComplete.SetActive(false);
        labelWaveComplete.SetActive(true);
    }

    public void ToggleShowGroupSupport()
    {
        if (groupSupportItems.activeInHierarchy)
        {
            groupSupportItems.SetActive(false);
        }
        else
        {
            groupSupportItems.SetActive(true);
        }
    }

    // Test
    public void NextWave()
    {
        ((SurvivalModeController)GameController.Instance.modeController).NextWave();
    }
}
