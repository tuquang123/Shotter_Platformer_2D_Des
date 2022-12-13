using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonSelectDifficulty : MonoBehaviour
{
    public Difficulty difficulty;
    public Image icon;
    public Image label;
    public Sprite sprIconNormal;
    public Sprite sprIconHighlight;
    public Sprite sprIconLock;
    public Sprite sprLabelNormal;
    public Sprite sprLabelHighlight;
    public Sprite sprLabelLock;

    public bool isLock;

    public void Highlight(bool isHighlight)
    {
        icon.sprite = isHighlight ? sprIconHighlight : sprIconNormal;
        icon.SetNativeSize();
        label.sprite = isHighlight ? sprLabelHighlight : sprLabelNormal;
        label.SetNativeSize();
    }

    public void Lock()
    {
        isLock = true;

        icon.sprite = sprIconLock;
        icon.SetNativeSize();
        label.sprite = sprLabelLock;
    }

    public void Select()
    {
        SoundManager.Instance.PlaySfxClick();

        if (isLock)
        {
            Popup.Instance.ShowToastMessage("Complete previous difficulty to unlock");
            return;
        }

        EventDispatcher.Instance.PostEvent(EventID.SelectDifficulty, difficulty);
    }
}
