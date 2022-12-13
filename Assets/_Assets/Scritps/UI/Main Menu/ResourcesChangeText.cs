using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourcesChangeText : MonoBehaviour
{
    public Text content;
    public RectTransform rectTransform;

    public void Active(bool isReceive, int value, Vector2 position, Transform parent = null)
    {
        content.color = isReceive ? Color.green : Color.red;
        string textForm = isReceive ? "+{0:n0}" : "-{0:n0}";
        content.text = string.Format(textForm, value);

        transform.SetParent(parent);
        rectTransform.position = position;
        rectTransform.localScale = Vector3.one;
        gameObject.SetActive(true);
    }

    public void Deactive()
    {
        Header.poolTextChange.Store(this);

        gameObject.SetActive(false);
    }
}
