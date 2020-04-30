using System.Threading;
using System.Threading.Tasks;

namespace Pis.Projekt.Framework.Repositories
{
    public interface IRepository<in TID, TEntityModel> 
        : IReadOnlyRepository<TID, TEntityModel>
        where TEntityModel : class, IEntity<TID>
    {
        Task<TEntityModel> CreateAsync(TEntityModel entity, CancellationToken token = default);

        Task<TEntityModel> UpdateAsync(TEntityModel entity, CancellationToken token = default);

        Task<bool> RemoveAsync(TID id, CancellationToken token = default);
    }
}