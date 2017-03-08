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

            /// <summary>
            /// Logs a message to the Unity Console
            /// </summary>
            /// <param name="msg">Message</param>
            [Conditional(_CONDITIONAL_STRING)]
            public static void Log(object msg)
            {
                UnityEngine.Debug.Log(msg);
            }

            /// <summary>
            /// Logs a formatted message to the Unity Console
            /// </summary>
            /// <param name="msg">Message</param>
            /// <param name="args">Arguments</param>
            [Conditional(_CONDITIONAL_STRING)]
            public static void LogFormat(string msg, params object[] args)
            {
                UnityEngine.Debug.LogFormat(msg, args);
            }

            /// <summary>
            /// Logs a warning message to the Unity Console
            /// </summary>
            /// <param name="msg">Message</param>
            [Conditional(_CONDITIONAL_STRING)]
            public static void LogWarning(object msg)
            {
                UnityEngine.Debug.LogWarning(msg);
            }

            /// <summary>
            /// Logs a formatted warning message to the Unity Console
            /// </summary>
            /// <param name="msg">Message</param>
            /// <param name="args">Arguments</param>
            [Conditional(_CONDITIONAL_STRING)]
            public static void LogWarningFormat(string msg, params object[] args)
            {
                UnityEngine.Debug.LogWarningFormat(msg, args);
            }

            /// <summary>
            /// Logs an error message to the Unity Console
            /// </summary>
            /// <param name="msg">Message</param>
            [Conditional(_CONDITIONAL_STRING)]
            public static void LogError(object msg)
            {
                UnityEngine.Debug.LogError(msg);
            }

            /// <summary>
            /// Logs a formatted error message to the Unity Console
            /// </summary>
            /// <param name="msg">Message</param>
            /// <param name="args">Arguments</param>
            [Conditional(_CONDITIONAL_STRING)]
            public static void LogErrorFormat(string msg, params object[] args)
            {
                UnityEngine.Debug.LogErrorFormat(msg, args);
            }

        }
    }
}