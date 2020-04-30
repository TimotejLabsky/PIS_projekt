using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Api.Controllers
{
    public class TaskController : Controller
    {
        // GET
        public async Task<IActionResult> ConfirmTaskAsync(Guid taskGuid, IEnumerable<PricedProduct> pricedProducts, CancellationToken token = default)
        {
            var task = _service.GetProductSalesDecreasedTask();
            task.Fulfill();
            return Ok();
        }

        private readonly Service _service;
    }
}