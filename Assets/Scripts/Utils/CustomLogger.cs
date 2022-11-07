using System;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class CustomLogger : MonoBehaviour
    {
        [Flags]
        public enum LogFlag
        {
            None = 0,
            AntController = 1 << 0,
            BehaviorTree = 1 << 1,
            Camera = 1 << 2,
            UserInput = 1 << 3,
            FoodPool = 1 << 4
        }

        [SerializeField]
        public static LogFlag currentLogs = LogFlag.None
            // | LogFlag.AntController
            // | LogFlag.BehaviorTree
            // | LogFlag.FoodPool
            ;

        public static void LogMessage(string message, LogFlag logFlag = LogFlag.None)
        {
#if UNITY_EDITOR
            if (currentLogs.HasFlag(logFlag))
                Debug.Log(message);
#endif
        }

        public static void LogWarning(string message, LogFlag logFlag = LogFlag.None)
        {
#if UNITY_EDITOR
            if (currentLogs.HasFlag(logFlag))
                Debug.LogWarning(message);
#endif
        }

        public static void LogError(string message, LogFlag logFlag = LogFlag.None)
        {
#if UNITY_EDITOR
            if (currentLogs.HasFlag(logFlag))
                Debug.LogError(message);
#endif
        }
    }
}