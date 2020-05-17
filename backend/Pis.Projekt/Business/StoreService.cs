using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Pis.Projekt.Business
{
    public class StoreService
    {
        public Task CheckStockings(string storeEmail)
        {
            _logger.LogBusinessCase($"Externa skladova sluzba pobocky {storeEmail} bola " +
                                    $"vyzvana na kontrolu skladovych zasob");
            return Task.CompletedTask;
        }

        private readonly ILogger<StoreService> _logger;

        public StoreService(ILogger<StoreService> logger)
        {
            _logger = logger;
        }
    }
}