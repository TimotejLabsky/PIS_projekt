using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Domain.DTOs;

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
        
        public Task SendOrder(ProductOrder order)
        {
            _logger.LogInformation($"Supplier received order of products: {order}");
            return Task.CompletedTask;
        }

        private readonly ILogger<SupplierService> _logger;
    }
}