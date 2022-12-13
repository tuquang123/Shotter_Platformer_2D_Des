using UnityEngine;
using System.Collections;

public class LoginLightEffect : MonoBehaviour
{
    public Animation anim;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 5f)
        {
            timer = 0;
            anim.Play();
        }
    }
}
