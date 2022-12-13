using UnityEngine;
using System.Collections;

public class TutorialGamePlayController : Singleton<TutorialGamePlayController>
{
    public TutorialStep[] mainSteps;

    private void Awake()
    {
        Init();
    }

    public void ShowTutorialBooster()
    {
        for (int i = 0; i < mainSteps.Length; i++)
        {
            mainSteps[i].Active(mainSteps[i].type == TutorialType.Booster);
        }
    }

    public void ShowTutorialActionIngame()
    {
        for (int i = 0; i < mainSteps.Length; i++)
        {
            mainSteps[i].Active(mainSteps[i].type == TutorialType.ActionInGame);
        }
    }

    public void ShowTutorialRecommendUpgradeWeapon()
    {
        for (int i = 0; i < mainSteps.Length; i++)
        {
            mainSteps[i].Active(mainSteps[i].type == TutorialType.RecommendUpgradeWeapon);
        }
    }

    public void ShowTutorialRecommendUpgradeCharacter()
    {
        for (int i = 0; i < mainSteps.Length; i++)
        {
            mainSteps[i].Active(mainSteps[i].type == TutorialType.RecommendUpgradeCharacter);
        }
    }

    private void Init()
    {
        for (int i = 0; i < mainSteps.Length; i++)
        {
            mainSteps[i].Init();
        }
    }
}
