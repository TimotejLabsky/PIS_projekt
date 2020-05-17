using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Domain.Repositories;
using Pis.Projekt.Domain.Repositories.Impl;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.System
{
    public class DatabaseWipe
    {
        public DatabaseWipe(IServiceScopeFactory scopeFactory,
            IOptions<DatabaseWipeConfiguration> configuration,
            ILogger<DatabaseWipeConfiguration> logger)
        {
            _configuration = configuration.Value;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task Wipe()
        {
            if (_configuration.WipeOnStart)
            {
                _logger.LogWarning("Wiping database on start...");
                using var scope = _scopeFactory.CreateScope();
                var pricedProductRepo =
                    scope.ServiceProvider.GetRequiredService<IPricedProductRepository>();
                var productRepo = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var seasonRepo = scope.ServiceProvider.GetRequiredService<ISeasonRepository>();
                var adRepo = scope.ServiceProvider.GetRequiredService<AdvertisedRepository>();

                await pricedProductRepo.WipeDatabase().ConfigureAwait(false);
                await productRepo.WipeDatabase().ConfigureAwait(false);
                await seasonRepo.WipeDatabase().ConfigureAwait(false);
                await adRepo.WipeDatabase().ConfigureAwait(false);
                _logger.LogWarning("Database wiped.");
            }
            else
            {
                _logger.LogWarning("Database wipe on start is disabled.");
            }
        }

        private readonly ILogger<DatabaseWipeConfiguration> _logger;
        private readonly DatabaseWipeConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public class DatabaseWipeConfiguration
        {
            public bool WipeOnStart { get; set; }
        }
    }

    public static class RepositoryWipeExtensions
    {
        public static async Task WipeDatabase<TID, TEntity>(
            this IRepository<TID, TEntity> repository)
            where TEntity : class, IEntity<TID>
        {
            var all = await repository.ListAsync().ConfigureAwait(false);
            foreach (var entity in all)
            {
                await repository.RemoveAsync(entity.Id).ConfigureAwait(false);
            }
        }
    }
}