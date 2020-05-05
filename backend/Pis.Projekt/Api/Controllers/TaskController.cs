using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Api.Controllers
{
    [Route("tasks")]
    public class TaskController : Controller
    {
        // GET
        [HttpPost("{taskGuid}")]
        public async Task<IActionResult> ConfirmTaskAsync([FromRoute] Guid taskGuid,
            [FromBody]IEnumerable<PricedProduct> pricedProducts,
            CancellationToken token = default)
        {
            var task = _taskCollection.Find(taskGuid);
            task.Fulfill(pricedProducts);
            return Ok();
        }

        private readonly UserTaskCollectionService _taskCollection;

        public TaskController(UserTaskCollectionService taskCollection)
        {
            _taskCollection = taskCollection;
        }
    }
}