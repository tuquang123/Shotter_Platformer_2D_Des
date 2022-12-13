using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestTab : MonoBehaviour
{
    public Image icon;
    public Image label;
    public Image bgButton;
    public Text notiCount;
    public Sprite sprIconEnable;
    public Sprite sprIconDisable;
    public Sprite sprLabelEnable;
    public Sprite sprLabelDisable;
    public Sprite sprBgButtonEnable;
    public Sprite sprBgButtonDisable;

    public void Highlight(bool isHighlight)
    {
        icon.sprite = isHighlight ? sprIconEnable : sprIconDisable;
        label.sprite = isHighlight ? sprLabelEnable : sprLabelDisable;
        bgButton.sprite = isHighlight ? sprBgButtonEnable : sprBgButtonDisable;
    }
}
