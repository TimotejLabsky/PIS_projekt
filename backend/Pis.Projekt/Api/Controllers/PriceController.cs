using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Api.Responses;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Api.Controllers
{
    [Route("price-list")]
    public class PriceController : Controller
    {
        public PriceController(SalesOptimalizationService optimizer,
            IPricedProductRepository pricedProductRepository,
            IMapper mapper,
            ILogger<PriceController> logger,
            SeasonService seasonService)
        {
            _optimizer = optimizer;
            _pricedProductRepository = pricedProductRepository;
            _mapper = mapper;
            _logger = logger;
            _seasonService = seasonService;
        }

        [HttpGet("{week}")]
        public async Task<ActionResult<ReadPriceListResponse>> ReadAsync(uint week,
            CancellationToken token = default)
        {
            var pricedEntities = await _pricedProductRepository.FetchFromWeekAsync(week, token)
                .ConfigureAwait(false);
            return Ok(pricedEntities.Select(p => _mapper.Map<PricedProductResponse>(p)));
        }

        [HttpGet("optimize")]
        public async Task<IActionResult> Optimize()
        {
            _logger.LogDebug("Manual Optimization process has started via Restful API call");
            await _optimizer.OptimizeSalesAsync().ConfigureAwait(false);
            _logger.LogDebug("Manual Optimization process started via Restful API call has ended");
            return Ok();
        }
        
        [HttpGet("season")]
        public async Task<IActionResult> HandleSeason()
        {
            _logger.LogDebug("Manual Seasonal process has started via Restful API call");
            await _seasonService.Handle().ConfigureAwait(false);
            _logger.LogDebug("Manual Seasonal process started via Restful API call has ended");
            return Ok();
        }

        private readonly IPricedProductRepository _pricedProductRepository;
        private readonly SalesOptimalizationService _optimizer;
        private readonly ILogger<PriceController> _logger;
        private readonly SeasonService _seasonService;
        private readonly IMapper _mapper;
    }
}