using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pis.Projekt.Business
{
    // Done
    public class WaiterService
    {
        public WaiterService(IOptions<WaiterConfiguration> configuration,
            ILogger<WaiterService> logger)
        {
            _logger = logger;
            _configuration = configuration.Value;
        }

        public async Task WaitAsync()
        {
            _logger.LogBusinessCase(BusinessTasks.WaitingTask);
            _token = new CancellationToken();
            // TODO: Production change to .From Hours(..)
            var hoursToWait = TimeSpan.FromMinutes(_configuration.WaitingHrsPeriod);
            await Task.Delay(hoursToWait, _token.Value)
                .ContinueWith(OnWaitingEnded);
        }
        
        
        public async Task WaitSeasonStartAsync()
        {
            _logger.LogBusinessCase(BusinessTasks.WaitingTaskSeasonStart);
            _token = new CancellationToken();
            // TODO: Production change to .From Hours(..)
            var hoursToWait = TimeSpan.FromMinutes(_configuration.SeasonStartWaitingHrs);
            await Task.Delay(hoursToWait, _token.Value)
                .ContinueWith(OnWaitingEnded);
        }
        

        public void Abort()
        {
            _token?.Register(() => { Console.WriteLine("Waiter aborted"); });
            _token = null;
        }

        private void OnWaitingEnded(Task waitingTask)
        {
            _logger.LogDebug("Waiting has ended");
        }


        private CancellationToken? _token;
        private readonly WaiterConfiguration _configuration;
        private readonly ILogger<WaiterService> _logger;

        public class WaiterConfiguration
        {
            public int WaitingHrsPeriod { get; set; }
            public int SeasonStartWaitingHrs { get; set; }
        }
    }
}