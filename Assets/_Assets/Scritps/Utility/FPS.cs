using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
#if UNITY_EDITOR
    private float fpsMeasurePeriod = 0.5f;
    private int fpsAccumulator = 0;
    private float fpsNextPeriod = 0;
    private int currentFps;
    private string display = "FPS: {0}";
    private Rect rect = new Rect(0f, 0f, 200f, 200f);

    void Start()
    {
        fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        fpsAccumulator++;

        if (Time.realtimeSinceStartup > fpsNextPeriod)
        {
            currentFps = (int)(fpsAccumulator / fpsMeasurePeriod);
            fpsAccumulator = 0;
            fpsNextPeriod += fpsMeasurePeriod;
        }
    }

    void OnGUI()
    {
        GUI.color = Color.green;
        GUI.Label(rect, string.Format(display, currentFps));
    }
#endif
}