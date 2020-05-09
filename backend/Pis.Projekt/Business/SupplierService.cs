using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Pis.Projekt.Business
{
    public class SupplierService
    {
        public SupplierService(ILogger<SupplierService> logger)
        {
            _logger = logger;
        }

        public Task EndProductStocking(IEnumerable<Guid> endProductsGuids)
        {
            _logger.LogInformation($"Stocking of products {endProductsGuids} has ended");
            return Task.CompletedTask;
        }
        
        public void SendOrder(ProductOrder order)
        {
            _logger.LogInformation($"Supplier received order of ptoducts: {order}");
        }

        private readonly ILogger<SupplierService> _logger;
    }
}