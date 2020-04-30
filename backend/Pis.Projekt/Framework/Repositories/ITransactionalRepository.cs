using System.Threading;
using System.Threading.Tasks;

namespace Pis.Projekt.Framework.Repositories
{
    public interface ITransactionalRepository
    {
        Task<object> BeginTransactionAsync(CancellationToken token = default);
        Task CommitTransactionAsync(object transaction, CancellationToken token = default);
        Task RollbackTransactionAsync(object transaction, CancellationToken token = default);
    }
}