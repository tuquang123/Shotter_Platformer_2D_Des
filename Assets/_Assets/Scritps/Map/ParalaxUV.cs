using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxUV : MonoBehaviour
{
    public float speed;
    public MeshRenderer render;

    private float lastCameraX;
    private Material mat;

    IEnumerator Start()
    {
        mat = render.sharedMaterial;

        yield return StaticValue.waitHalfSec;

        lastCameraX = Camera.main.transform.position.x;
    }

    void Update()
    {
        if (GameController.Instance.Player == null)
            return;

        float sign = -Mathf.Sign(Camera.main.transform.position.x - lastCameraX);

        //if (lastCameraX != Camera.main.transform.position.x && GameController.Instance.Player.IsMoving)
        if (Mathf.Abs(lastCameraX - Camera.main.transform.position.x) > 0.02f && GameController.Instance.Player.IsMoving)
        {
            lastCameraX = Camera.main.transform.position.x;

            Vector2 v = mat.mainTextureOffset;
            v.x += sign * speed * Time.deltaTime;
            mat.mainTextureOffset = v;
        }
    }
}
