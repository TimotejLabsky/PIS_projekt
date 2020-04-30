using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pis.Projekt.Framework.Repositories
{
    public abstract class AbstractRepository<TKey, TDocument> : IRepository<TKey, TDocument>
        where TDocument : class, IEntity<TKey>
    {
        public abstract Task<IEnumerable<TDocument>> ListAsync(
            Expression<Func<TDocument, bool>> predicate =
                null,
            string sortField = null,
            bool isAscending = true,
            CancellationToken token = default);

        public abstract Task<TDocument> FindAsync(TKey id, CancellationToken token = default);

        public abstract Task<TDocument> RequireAsync(TKey id, CancellationToken token = default);

        public abstract Task<TDocument> CreateAsync(TDocument document, CancellationToken token = default);

        public abstract Task<TDocument> UpdateAsync(TDocument document,
            CancellationToken token = default);

        public abstract Task<bool> RemoveAsync(TKey id, CancellationToken token = default);

        public abstract void UpdateMerge(TDocument updatedEntity, TDocument updatingEntity);
    }
}