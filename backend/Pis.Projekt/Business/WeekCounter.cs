
using Microsoft.Extensions.Configuration;

namespace Pis.Projekt.Business
{
    // Done
    public class WeekCounter
    {
        public WeekCounter(IConfiguration configuration)
        {
            _weekNumber = configuration.GetValue<uint>("WeekNumber");
        }

        public uint Next()
        {
            // TODO maybe semaphore locking
            return _weekNumber++;
        }
        
        public uint Current()
        {
            return _weekNumber;
        }
        
        private uint _weekNumber;
    }
}