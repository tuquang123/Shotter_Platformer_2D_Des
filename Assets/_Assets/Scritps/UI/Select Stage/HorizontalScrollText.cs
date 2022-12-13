using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HorizontalScrollText : MonoBehaviour
{
    public Text textContent;
    public Vector2 mostRightPoint;
    public float viewSize;

    private Vector2 initialPoint;
    private Vector2 mostLeftPoint;
    [SerializeField]
    private bool isScrollable;
    [SerializeField]
    private bool flagScroll;

    private void Awake()
    {
        textContent = GetComponent<Text>();

        initialPoint = textContent.rectTransform.anchoredPosition;
    }

    void Update()
    {
        if (isScrollable && flagScroll)
        {
            textContent.rectTransform.anchoredPosition = Vector2.MoveTowards(textContent.rectTransform.anchoredPosition, mostLeftPoint, Time.deltaTime * 60);

            if (textContent.rectTransform.anchoredPosition == mostLeftPoint)
            {
                textContent.rectTransform.anchoredPosition = mostRightPoint;
            }
        }
    }

    public void Active(string content)
    {
        flagScroll = false;
        textContent.text = content;
        isScrollable = textContent.preferredWidth > viewSize;

        if (isScrollable)
        {
            mostLeftPoint.x = mostRightPoint.x - viewSize - textContent.preferredWidth;
            mostLeftPoint.y = textContent.rectTransform.anchoredPosition.y;

            enabled = true;
            Invoke("ActiveFlagScroll", 1.5f);
        }
    }

    public void Deactive()
    {
        flagScroll = false;
        textContent.text = string.Empty;

        if (isScrollable)
        {
            textContent.rectTransform.anchoredPosition = initialPoint;

            isScrollable = false;
            enabled = false;
        }
    }

    private void ActiveFlagScroll()
    {
        flagScroll = true;
    }
}
