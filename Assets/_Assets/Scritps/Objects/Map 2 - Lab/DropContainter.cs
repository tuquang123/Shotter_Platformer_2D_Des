using UnityEngine;
using System.Collections;

public class DropContainter : MonoBehaviour
{
    private Rigidbody2D rigid;
    private bool isGrounded;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            Drop();
        }
        else if (other.transform.root.CompareTag(StaticValue.TAG_MAP))
        {
            if (isGrounded == false)
            {
                isGrounded = true;
                SetDefault();
                CameraFollow.Instance.AddShake(0.5f, 0.3f);

                SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_TRIGGER_WATER);
            }
        }
    }

    private void Drop()
    {
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.useAutoMass = true;
    }

    private void SetDefault()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic;
    }
}
