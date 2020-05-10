using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Api.Requests;
using Pis.Projekt.Api.Responses;
using Pis.Projekt.Business;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Api.Controllers
{
    [Route("tasks")]
    public class TaskController : Controller
    {
        public TaskController(UserTaskCollectionService taskCollection,
            ILogger<TaskController> logger,
            IMapper mapper,
            ITaskClient client,
            IProductRepository productRepository)
        {
            _taskCollection = taskCollection;
            _logger = logger;
            _mapper = mapper;
            _client = client;
            _productRepository = productRepository;
        }

        [HttpPost("fulfill")]
        public async Task<IActionResult> ConfirmTaskAsync([FromBody] TaskFulfillRequest request)
        {
            try
            {
                var task = _taskCollection.Find(request.Id);
                var taskProducts = request.Products.Select(p =>
                {
                    var taskProduct = _mapper.Map<TaskProduct>(p);
                    var productEntity = _productRepository.RequireAsync(p.ProductId).Result;
                    taskProduct.Product = _mapper.Map<Product>(productEntity);
                    taskProduct.Price = p.NewPrice;    
                    return taskProduct;
                }).ToList();
                
                
                task.Key.Fulfill(taskProducts);
                _taskCollection.Fulfill(request.Id);
                await _client.SetCompleteAsync(task.Value).ConfigureAwait(false);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Unable to fulfill task {request.Id}: {request.Name}. Failed on: {e}");
                return NoContent();
            }
        }

        [HttpGet("next")]
        public ActionResult<NextTaskResponse> GetNext()
        {
            try
            {
                var next = _taskCollection.GetNext();
                var responseTask = _mapper.Map<NextTaskResponse>(next);
                return Ok(responseTask);
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
        private readonly IProductRepository _productRepository;
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskClient _client;
        private readonly IMapper _mapper;
    }
}