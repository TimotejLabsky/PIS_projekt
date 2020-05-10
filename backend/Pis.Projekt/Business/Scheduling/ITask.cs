using System.Collections.Generic;
using System.Threading.Tasks;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Scheduling
{
    public interface ITask<TResult>
    {
        string Name { get; }
        IEnumerable<TaskProduct> Result { get; set; }
    }
}