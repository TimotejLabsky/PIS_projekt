using Microsoft.Extensions.Logging;

namespace Pis.Projekt.Business
{
    public static class BusinessLoggingExtensions
    {
        public static void LogBusinessCase<TLogger>(this ILogger<TLogger> logger,
            string message,
            object data = null)
        {
            logger.LogInformation($"Business - {message}", data);
        }
    }
}