using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pis.Projekt.Framework.Repositories
{
    public interface IReadOnlyRepository<in TID, TEntityModel>
        where TEntityModel : class, IEntity<TID>
    {
        Task<IEnumerable<TEntityModel>> ListAsync(
            Expression<Func<TEntityModel, bool>> predicate = null,
            string sortField = null, bool isAscending = true,
            CancellationToken token = default);

        Task<TEntityModel> FindAsync(TID id, CancellationToken token = default);

        Task<TEntityModel> RequireAsync(TID id, CancellationToken token = default);
    }
}