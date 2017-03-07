using System.Diagnostics;

namespace Framework
{
    namespace Log
    {
        /// <summary>
        /// Wrapper class for Unity's logger
        /// </summary>
        public static class CustomLogger
        {
            #region Variables
            private const string _CONDITIONAL_STRING = "UNITY_EDITOR";
            #endregion

            [Conditional(_CONDITIONAL_STRING)]
            public static void Log(object msg)
            {
                UnityEngine.Debug.Log(msg);
            }

            [Conditional(_CONDITIONAL_STRING)]
            public static void LogFormat(string msg, params object[] args)
            {
                UnityEngine.Debug.LogFormat(msg, args);
            }

            [Conditional(_CONDITIONAL_STRING)]
            public static void LogWarning(object msg)
            {
                UnityEngine.Debug.LogWarning(msg);
            }

            [Conditional(_CONDITIONAL_STRING)]
            public static void LogWarningFormat(string msg, params object[] args)
            {
                UnityEngine.Debug.LogWarningFormat(msg, args);
            }

            [Conditional(_CONDITIONAL_STRING)]
            public static void LogError(object msg)
            {
                UnityEngine.Debug.LogError(msg);
            }

            [Conditional(_CONDITIONAL_STRING)]
            public static void LogErrorFormat(string msg, params object[] args)
            {
                UnityEngine.Debug.LogErrorFormat(msg, args);
            }

        }
    }
}