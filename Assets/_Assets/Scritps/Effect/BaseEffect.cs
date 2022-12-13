using UnityEngine;
using System.Collections;

public class BaseEffect : MonoBehaviour
{
    public virtual void Active(Vector3 position, Transform parent = null)
    {
        transform.position = position;
        transform.parent = parent;

        gameObject.SetActive(true);
    }

    public virtual void Deactive()
    {
        gameObject.SetActive(false);
    }
}
