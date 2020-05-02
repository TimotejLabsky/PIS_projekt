using Microsoft.Extensions.Logging;

namespace Pis.Projekt.System
{
    public static class LoggerDevelopmentExtensions
    {
        public static void LogDevelopment<TLogger>(this ILogger<TLogger> logger,
            string message,
            object data = null)
        {
            logger.LogWarning($"!! DEVELOPMENT !! - {message}", data);
        }
    }
}