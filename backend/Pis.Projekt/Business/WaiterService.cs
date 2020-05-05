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
        public WaiterService(IOptions<WaiterConfiguration> configuration, ILogger<WaiterService> logger)
        {
            _logger = logger;
            _configuration = configuration.Value;
        }
        
        public async Task WaitAsync()
        {
            _token = new CancellationToken();
            await Task.Delay(_configuration.WaitingPeriod, _token.Value)
                .ContinueWith(OnWaitingEnded);
        }

        public void Abort()
        {
            _token?.Register(() => { Console.WriteLine("Waiter aborted"); });
            _token = null;
        }

        private void OnWaitingEnded(Task waitingTask)
        {
            _logger.LogDebug("Water has ended");
        }


        private CancellationToken? _token;
        private readonly WaiterConfiguration _configuration;
        private readonly ILogger<WaiterService> _logger;

        public class WaiterConfiguration
        {
            public TimeSpan WaitingPeriod { get; set; }
        }
    }
}