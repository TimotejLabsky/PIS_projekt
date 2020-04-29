using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.Database.Contexts;
using Pis.Projekt.Framework;

namespace Pis.Projekt.Domain.Repositories
{
    public class SalesAggregateRepository: EFRepository<SalesDbContext, uint, SalesAggregateEntity>
    {
        public SalesAggregateRepository(SalesDbContext context) : base(context)
        {
            // empty
        }

        public async Task<IEnumerable<SalesAggregateEntity>> FetchFromLastWeek()
        {
            var a = _counter.Current();
            throw new NotImplementedException();
        }
        
        private readonly WeekCounter _counter;

    }
}