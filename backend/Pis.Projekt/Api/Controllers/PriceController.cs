using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Api.Responses;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.Database.Contexts;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Api.Controllers
{
    [Route("price-list")]
    public class PriceController : Controller
    {
        public PriceController(SalesOptimalizationService optimizer,
            SalesDbContext dbContext,
            IPricedProductRepository pricedProductRepository,
            IMapper mapper)
        {
            _optimizer = optimizer;
            _dbContext = dbContext;
            _pricedProductRepository = pricedProductRepository;
            _mapper = mapper;
        }

        [HttpGet("{week}")]
        public async Task<ActionResult<ReadPriceListResponse>> ReadAsync(uint week,
            CancellationToken token = default)
        {
            var pricedEntities = await _pricedProductRepository.FetchFromWeekAsync(week, token)
                .ConfigureAwait(false);
            return Ok(pricedEntities.Select(p => _mapper.Map<PricedProductResponse>(p)));
        }

        [HttpGet("evaluate")]
        public async Task<IActionResult> Evaluate()
        {
            await _optimizer.OptimizeSalesAsync().ConfigureAwait(false);
            return Ok();
        }

        private readonly IPricedProductRepository _pricedProductRepository;
        private readonly SalesOptimalizationService _optimizer;
        private readonly ILogger<PriceController> _logger
        private readonly SalesDbContext _dbContext;
        private readonly IMapper _mapper;
    }
}