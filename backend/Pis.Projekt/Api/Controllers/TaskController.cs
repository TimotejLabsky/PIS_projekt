using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Api.Controllers
{
    [Route("tasks")]
    public class TaskController : Controller
    {
        public TaskController(UserTaskCollectionService taskCollection,
            ILogger<TaskController> logger)
        {
            _taskCollection = taskCollection;
            _logger = logger;
        }

        [HttpPost("{taskGuid}")]
        public async Task<IActionResult> ConfirmTaskAsync([FromRoute] Guid taskGuid,
            [FromBody] IEnumerable<PricedProduct> pricedProducts,
            CancellationToken token = default)
        {
            var task = _taskCollection.Find(taskGuid);
            task.Fulfill(pricedProducts);
            return Ok();
        }

        [HttpGet("next")]
        public IActionResult GetNext()
        {
            try
            {
                var task = _taskCollection.GetNext();
                return Ok(task);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation("Next task does not exist", e);
                return NotFound();
            }
        }

        [HttpGet("list")]
        public IActionResult GetAll()
        {
            var all = _taskCollection.All();
            return Ok(all);
        }

        private readonly UserTaskCollectionService _taskCollection;
        private readonly ILogger<TaskController> _logger;
    }
}