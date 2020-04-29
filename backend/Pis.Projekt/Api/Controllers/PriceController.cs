using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pis.Projekt.Api.Responses;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.Database.Contexts;

namespace Pis.Projekt.Api.Controllers
{
    [Route("price-list")]
    public class PriceController : Controller
    {
        public PriceController(SalesOptimalizationService optimizer, SalesDbContext dbContext)
        {
            _optimizer = optimizer;
            _dbContext = dbContext;
        }
        
        [HttpGet("{week}")]
        public async Task<ActionResult<ReadPriceListResponse>> Read(int week)
        {
            return Ok(_dbContext.PriceList);
        }
        
        [HttpGet("evaluate")]
        public async Task<IActionResult> Evaluate()
        {
            await _optimizer.OptimizeSalesAsync().ConfigureAwait(false);
            return Ok();
        }

        private readonly SalesOptimalizationService _optimizer;
        private readonly SalesDbContext _dbContext;
    }
}