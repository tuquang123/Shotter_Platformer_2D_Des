using UnityEngine;
using System.Collections;

public class TutorialStep : MonoBehaviour
{
    public TutorialType type;
    public TutorialSubStep[] subSteps;

    private int curSubStepIndex;

    public virtual void Init()
    {
        curSubStepIndex = 0;

        for (int i = 0; i < subSteps.Length; i++)
        {
            subSteps[i].type = type;
            subSteps[i].stepIndex = i;
            subSteps[i].Init();
        }

        EventDispatcher.Instance.RegisterListener(EventID.CompleteSubStep, (sender, param) => OnSubStepComplete((TutorialSubStepData)param));
        EventDispatcher.Instance.RegisterListener(EventID.CompleteStep, (sender, param) =>
        {
            if ((TutorialType)param == type)
            {
                Complete();
            }
        });
    }

    public virtual void Active(bool isActive)
    {
        if (isActive)
        {
            GameData.isShowingTutorial = true;
            gameObject.SetActive(true);
            StartCurrentSubStep();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void Skip()
    {
        Popup.Instance.Show(
            content: "Do you want to skip this tutorial?",
            title: "tutorial",
            type: PopupType.YesNo,
            yesCallback: () =>
            {
                Complete();
            });
    }

    public virtual void Complete()
    {
        GameData.isShowingTutorial = false;
        GameData.playerTutorials.SetComplete(type);
        Active(false);
    }

    public virtual void OnSubStepComplete(TutorialSubStepData data)
    {
        if (data.type == type)
        {
            if (curSubStepIndex == subSteps.Length - 1)
            {
                Complete();
            }
            else
            {
                curSubStepIndex++;
                StartCurrentSubStep();
            }
        }
    }

    private void StartCurrentSubStep()
    {
        for (int i = 0; i < subSteps.Length; i++)
        {
            if (curSubStepIndex == i)
            {
                subSteps[i].StartSubStep();
            }
            else
            {
                subSteps[i].gameObject.SetActive(false);
            }
        }
    }
}
