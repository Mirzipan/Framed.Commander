using UnityEngine;

namespace Mirzipan.Heist
{
    public static class HeistLogger
    {
        private const string CriticalColor = nameof(Color.magenta);
        private const string ErrorColor = nameof(Color.red);
        private const string WarningColor = nameof(Color.yellow);
        private const string InfoColor = nameof(Color.green);
        private const string DebugColor = nameof(Color.white);

        public static void Critical(string tag, object message)
        {
            UnityEngine.Debug.LogError(Format(CriticalColor, tag, message));
        }

        public static void CriticalFormat(string tag, string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(Format(CriticalColor, tag, string.Format(format, args)));
        }

        public static void Error(string tag, object message)
        {
            UnityEngine.Debug.LogError(Format(ErrorColor, tag, message));
        }

        public static void ErrorFormat(string tag, string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(Format(ErrorColor, tag, string.Format(format, args)));
        }

        public static void Warning(string tag, object message)
        {
            UnityEngine.Debug.LogWarning(Format(WarningColor, tag, message));
        }

        public static void WarningFormat(string tag, string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(Format(WarningColor, tag, string.Format(format, args)));
        }

        public static void Info(string tag, object message)
        {
            UnityEngine.Debug.Log(Format(InfoColor, tag, message));
        }

        public static void InfoFormat(string tag, string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(Format(InfoColor, tag, string.Format(format, args)));
        }

        public static void Debug(string tag, object message)
        {
            UnityEngine.Debug.Log(Format(DebugColor, tag, message));
        }

        public static void DebugFormat(string tag, string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(Format(DebugColor, tag, string.Format(format, args)));
        }

        private static string Format(string color, string tag, object message)
        {
            return $"<color={color}><b>[{tag}]</b> {message}</color>";
        }
    }
}