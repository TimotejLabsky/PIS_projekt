using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Repositories
{
    public interface IProductRepository : IRepository<Guid, ProductEntity>,
        ITransactionalRepository
    {
        // empty
    }
}