using UnityEngine;
using System.Collections;

public class DebugLog {
    public static void Log(string logInfo)
    {
        if(AppConfig.GetInstance().IsDebugMode())
        {
            Debug.Log(logInfo);
        }
    }

    public static void LogFile(string logInfo)
    {

    }
}
