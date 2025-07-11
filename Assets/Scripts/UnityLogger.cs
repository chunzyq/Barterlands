using UnityEngine;

namespace Barterlands.Logging
{
    public class UnityLogger : ILoggerService
    {
        private static readonly string InfoTag = "<color=cyan><b>[INFO]</b></color>";
        private static readonly string WarningTag = "<color=yellow><b>[WARNING]</b></color>";
        private static readonly string ErrorTag = "<color=red><b>[ERROR]</b></color>";

        public void Info(string message, Object context = null)
        {
            Debug.Log($"{InfoTag} {message}", context);
        }

        public void Warning(string message, Object context = null)
        {
            Debug.LogWarning($"{WarningTag} {message}", context);
        }

        public void Error(string message, Object context = null)
        {
            Debug.LogError($"{ErrorTag} {message}", context);
        }
    }
}
