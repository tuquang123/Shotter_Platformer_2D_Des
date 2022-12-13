using UnityEngine;
using System.Collections;

public class AutoMoveBar : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            other.transform.root.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.CompareTag(StaticValue.TAG_PLAYER))
        {
            other.transform.parent = null;
        }
    }
}
