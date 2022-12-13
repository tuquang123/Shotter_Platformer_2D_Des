using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonActionIngame : MonoBehaviour
{
    public Button button;
    public Sprite sprDisable;
    public Sprite sprNormal;
    public float sizeNormal;
    public float sizePress;

    private bool isDisabled;

    public void Normal()
    {
        SetSizeToNormal();

        if (isDisabled)
        {
            SetAlpha(0.4f);
        }
        else
        {
            button.image.sprite = sprNormal;
            SetAlpha(0.6f);
        }
    }

    public void Press()
    {
        SetSizeToPress();
        SetAlpha(1f);
    }

    public void Enable()
    {
        isDisabled = false;

        button.enabled = true;
        button.image.raycastTarget = true;
        Normal();
    }

    public void Disable()
    {
        isDisabled = true;

        button.enabled = false;
        button.image.raycastTarget = false;

        if (sprDisable)
            button.image.sprite = sprDisable;

        SetSizeToNormal();
        SetAlpha(0.4f);
    }

    private void SetSizeToNormal()
    {
        Vector2 v = button.image.rectTransform.sizeDelta;
        v.x = sizeNormal;
        v.y = sizeNormal;
        button.image.rectTransform.sizeDelta = v;
    }

    private void SetSizeToPress()
    {
        Vector2 v = button.image.rectTransform.sizeDelta;
        v.x = sizePress;
        v.y = sizePress;
        button.image.rectTransform.sizeDelta = v;
    }

    private void SetAlpha(float a)
    {
        Color c = button.image.color;
        c.a = a;
        button.image.color = c;
    }
}
