using UnityEngine;

public static class Logs {
    private static bool enabled = false;

    public static bool Enabled {
        get {
#if UNITY_EDITOR || DEBUG
            return true;
#else
            return enabled;
#endif
        }
    }

    public static void SetState(bool enable) {
        enabled = enable;
    }

    public static void Log(object message) {
        if (Enabled) Debug.Log(message);
    }

    public static void Log(object message, Object context) {
        if (Enabled) Debug.Log(message, context);
    }

    public static void LogWarning(object message) {
        if (Enabled) Debug.LogWarning(message);
    }

    public static void LogWarning(object message, Object context) {
        if (Enabled) Debug.LogWarning(message, context);
    }

    public static void LogError(object message) {
        Debug.LogError(message);
    }

    public static void LogError(object message, Object context) {
        Debug.LogError(message, context);
    }
}
