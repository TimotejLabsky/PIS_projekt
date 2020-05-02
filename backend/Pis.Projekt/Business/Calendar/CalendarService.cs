using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using FiitCalendarService;

namespace Pis.Projekt.Business.Calendar
{
    public class CalendarService
    {
        public CalendarService(CalendarPortTypeClient calendarPortTypeClient)
        {
            _calendarPortTypeClient = calendarPortTypeClient;
        }
        
        public async Task<DateTime> GetCurrentDate()
        {
            DateTime outDate;
            var currentDate = await _calendarPortTypeClient.getCurrentDateAsync().ConfigureAwait(false);
            if (!DateTime.TryParseExact(currentDate.date, "YYYY-MM-DD",
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, out outDate))
            {
                throw new InvalidDataException($"Unable to parse date {currentDate.date}");
            }

            return outDate;
        }

        private readonly FiitCalendarService.CalendarPortTypeClient _calendarPortTypeClient;

     
    }
}