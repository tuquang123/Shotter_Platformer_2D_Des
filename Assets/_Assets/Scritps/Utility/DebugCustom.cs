using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class DebugCustom
{
    [Conditional("DEBUG_ENABLE")]
    public static void Log(object content)
    {
        Debug.Log(content.ToString());
    }

    [Conditional("DEBUG_ENABLE")]
    public static void LogError(object content)
    {
        Debug.LogError(content.ToString());
    }

    [Conditional("DEBUG_ENABLE")]
    public static void LogWarning(object content)
    {
        Debug.LogWarning(content.ToString());
    }

    [Conditional("DEBUG_ENABLE")]
    public static void LogException(Exception e)
    {
        Debug.LogException(e);
    }
}
