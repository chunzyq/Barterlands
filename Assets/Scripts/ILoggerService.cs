using UnityEngine;

namespace Barterlands.Logging
{
    public interface ILoggerService
    {
        void Info(string message, Object context = null);
        void Warning(string message, Object context = null);
        void Error(string message, Object context = null);
    }
}
