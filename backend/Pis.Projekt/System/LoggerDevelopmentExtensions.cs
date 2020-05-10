using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Pis.Projekt.System
{
    public static class LoggerDevelopmentExtensions
    {
        public static void LogDevelopment<TLogger>(this ILogger<TLogger> logger,
            string message,
            object data = null)
        {
            if (data != null)
            {
                var jsonData = JsonConvert.SerializeObject(data);
                logger.LogWarning($"!! DEVELOPMENT !! - {message}\n" +
                                  $"Data: {jsonData}");
            }
            else
            {
                logger.LogWarning($"!! DEVELOPMENT !! - {message}");

            }
        }
    }
}