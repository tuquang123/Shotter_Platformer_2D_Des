using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantLoading : MonoBehaviour
{
    [SerializeField]
    private Text labelTimeout;
    private int timeout = 15;

    public void Show(int timeout)
    {
        StopAllCoroutines();

        this.timeout = timeout;
        labelTimeout.text = timeout.ToString();
        gameObject.SetActive(true);
        StartCoroutine(Timeout());
    }

    public void Hide()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    IEnumerator Timeout()
    {
        while (timeout > 0)
        {
            yield return StaticValue.waitOneSec;
            timeout--;
            labelTimeout.text = timeout.ToString();
        }

        gameObject.SetActive(false);
    }
}
