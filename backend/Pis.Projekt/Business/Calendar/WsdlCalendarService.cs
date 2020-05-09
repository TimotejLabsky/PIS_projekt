using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using FiitCalendarService;
using Microsoft.Extensions.Logging;

namespace Pis.Projekt.Business.Calendar
{
    public class WsdlCalendarService
    {
        public WsdlCalendarService(CalendarPortTypeClient client, ILogger<WsdlCalendarService> logger)
        {
            _client = client;
            _logger = logger;
        }
        
        public async Task<DateTime> GetCurrentDateAsync()
        {
            var currentDate = await _client.getCurrentDateAsync().ConfigureAwait(false);
            if (!DateTime.TryParseExact(currentDate.date, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, out var outDate))
            {
                throw new InvalidDataException($"Unable to parse date {currentDate.date}");
            }
            _logger.LogDebug($"Successfully got today's date {outDate}");
            return outDate;
        }

        private readonly CalendarPortTypeClient _client;
        private readonly ILogger<WsdlCalendarService> _logger;


    }
}