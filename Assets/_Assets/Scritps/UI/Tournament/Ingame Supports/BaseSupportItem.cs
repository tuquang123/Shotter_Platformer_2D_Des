using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseSupportItem : MonoBehaviour
{
    public Button icon;
    public GameObject groupFree;
    public GameObject groupPrice;
    public Text textPrice;

    protected int countUsed;
    protected int priceUse;


    public virtual void Init()
    {
        EventDispatcher.Instance.RegisterListener(EventID.CompleteWave, (sender, param) => OnCompleteWave());

        icon.onClick.AddListener(Consume);
        Active(true);
    }

    protected virtual void OnCompleteWave() { }

    protected virtual void Consume()
    {
        countUsed++;
    }

    protected virtual void Active(bool isActive)
    {
        icon.interactable = isActive;

        if (isActive == false)
        {
            groupFree.SetActive(false);
            groupPrice.SetActive(false);
        }
    }
}
