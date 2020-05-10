using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Api.Requests;
using Pis.Projekt.Api.Responses;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Api.Controllers
{
    [Route("tasks")]
    public class TaskController : Controller
    {
        public TaskController(UserTaskCollectionService taskCollection,
            ILogger<TaskController> logger,
            IMapper mapper)
        {
            _taskCollection = taskCollection;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("fulfill")]
        public async Task<IActionResult> ConfirmTaskAsync([FromBody] TaskFulfillRequest request,
            CancellationToken token = default)
        {
            try
            {
                var task = _taskCollection.Find(request.Id);
                var taskProducts = request.Products.Select(p => _mapper.Map<TaskProduct>(p));
                task.Fulfill(taskProducts);
                _taskCollection.Fulfill(request.Id);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Unable to fulfill task {request.Id}: {request.Name}", e);
                return NoContent();
            }
        }

        [HttpGet("next")]
        public ActionResult<NextTaskResponse> GetNext()
        {
            try
            {
                var task = _mapper.Map<NextTaskResponse>(_taskCollection.GetNext());
                return Ok(task);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation("Next task does not exist", e);
                return NoContent();
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
        private readonly IMapper _mapper;
    }
}