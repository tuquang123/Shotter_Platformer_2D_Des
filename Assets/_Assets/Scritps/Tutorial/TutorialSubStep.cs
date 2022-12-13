using UnityEngine;
using System.Collections;

public class TutorialSubStep : MonoBehaviour
{
    public TutorialType type;
    public int stepIndex;

    public virtual void Init()
    {

    }

    public virtual void StartSubStep()
    {
        gameObject.SetActive(true);
    }

    public virtual void Next()
    {
        SoundManager.Instance.PlaySfxClick();

        TutorialSubStepData step = new TutorialSubStepData(type, stepIndex);
        EventDispatcher.Instance.PostEvent(EventID.CompleteSubStep, step);

        gameObject.SetActive(false);
    }
}

public class TutorialSubStepData
{
    public TutorialType type;
    public int stepIndex;

    public TutorialSubStepData(TutorialType type, int stepIndex)
    {
        this.type = type;
        this.stepIndex = stepIndex;
    }
}
