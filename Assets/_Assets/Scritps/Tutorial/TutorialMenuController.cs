using UnityEngine;
using System.Collections;

public class TutorialMenuController : Singleton<TutorialMenuController>
{
    public TutorialStep[] mainSteps;

    private void Awake()
    {
        Init();
    }

    public void ShowTutorial(TutorialType type)
    {
        for (int i = 0; i < mainSteps.Length; i++)
        {
            mainSteps[i].Active(mainSteps[i].type == type);
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
