using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedUvMaterial : MonoBehaviour
{
    public Vector2 offsetSpeed;
    public Material mat;

    private Vector2 defaultUVOffset;
    private float limitOffsetX, limitOffsetY;

    void Start()
    {
        if (mat == null)
        {
            enabled = false;
            return;
        }

        defaultUVOffset = mat.mainTextureOffset;
        limitOffsetX = defaultUVOffset.x + 10f;
        limitOffsetY = defaultUVOffset.y + 10f;
    }

    void Update()
    {
        Vector2 v = mat.mainTextureOffset;
        v += offsetSpeed * Time.deltaTime;

        if (v.x >= limitOffsetX || v.x <= -limitOffsetX)
            v.x = defaultUVOffset.x;

        if (v.y >= limitOffsetY || v.y <= -limitOffsetY)
            v.y = defaultUVOffset.y;

        mat.mainTextureOffset = v;
    }

    void OnDisable()
    {
        if (mat != null)
        {
            mat.mainTextureOffset = defaultUVOffset;
        }
    }
}
