using UnityEngine;
using System.Collections;

public class MapOverview : MonoBehaviour
{
    public StageButton[] stageButtons;

    public void Init()
    {
        Load();
    }

    public void Active(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private void Load()
    {
        for (int i = 0; i < stageButtons.Length; i++)
        {
            StageButton stage = stageButtons[i];

            stage.Load();
        }
    }
}
