using UnityEngine;
using System.Collections;

public class AnimatedUV : MonoBehaviour
{
    public Vector2 offsetSpeed;

    private Vector2 defaultUVOffset;
    private Material mat;
    private float limitOffsetX, limitOffsetY;

    void Start()
    {
        mat = GetComponent<Renderer>().sharedMaterial;

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
