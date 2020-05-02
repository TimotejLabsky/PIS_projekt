using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Pis.Projekt.Business
{
    // Done
    public class WaiterService
    {
        public async Task WaitAsync()
        {
            _token = new CancellationToken();
            await Task.Delay(_configuration.WaitingPeriod, _token.Value);
        }

        public void Abort()
        {
            _token?.Register(() =>
            {
                Console.WriteLine("Waiter aborted");
            });
            _token = null;
        }

        private CancellationToken? _token;
        private readonly WaiterConfiguration _configuration;

        public WaiterService(IOptions<WaiterConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public class WaiterConfiguration {
            public TimeSpan WaitingPeriod { get; set; }
        }
    }
}