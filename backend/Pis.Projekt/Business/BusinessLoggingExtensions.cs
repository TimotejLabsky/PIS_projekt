using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz.Util;

namespace Pis.Projekt.Business
{
    public static class BusinessLoggingExtensions
    {
        public static void LogBusinessCase<TLogger>(this ILogger<TLogger> logger,
            string taskName,
            string message = null,
            object data = null)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.Append($"Business - Task: {taskName}");
            if (message.TrimEmptyToNull() != null)
            {
                messageBuilder.Append($"\nMessage: {message}");
            }
            
            logger.LogInformation(messageBuilder.ToString(), data);
        }
        
        public static void LogInput<TLogger>(this ILogger<TLogger> logger,
            string taskName,
            string inputName,
            object data = null)
        {
            if (data != null)
            {
                var dataAsJson = JsonConvert.SerializeObject(data, Formatting.Indented);
                data = dataAsJson;
            }

            logger.LogInformation($"Business - Task: {taskName} - Input\n" +
                                  $"{inputName}: {data}", data);
        }
        
        public static void LogOutput<TLogger>(this ILogger<TLogger> logger,
            string taskName,
            string outputName,
            object data = null)
        {
            if (data != null)
            {
                var dataAsJson = JsonConvert.SerializeObject(data, Formatting.Indented);
                data = dataAsJson;
            }
            
            logger.LogInformation($"Business - Task: {taskName} - Output\n" +
                                  $"{outputName}: {data}");
        }
    }
}