using Firebase.Analytics;
using System;
using System.Collections.Generic;

public class FirebaseAnalyticsHelper
{
    public static void LogScreen(string screen)
    {
        FirebaseAnalytics.SetCurrentScreen(screen, screen);
    }

    public static void LogEvent(string eventName, params object[] args)
    {
#if !UNITY_EDITOR
        try
        {
            List<Parameter> listparams = new List<Parameter>();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is int)
                {
                    listparams.Add(new Parameter("Param" + i, (int)args[i]));
                }
                else if (args[i] is long)
                {
                    listparams.Add(new Parameter("Param" + i, (long)args[i]));
                }
                else
                {
                    listparams.Add(new Parameter("Param" + i, args[i].ToString()));
                }
            }

            FirebaseAnalytics.LogEvent(eventName, listparams.ToArray());
        }
        catch (Exception e)
        {
            DebugCustom.LogError(e);
        }
#endif
    }
}