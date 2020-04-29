using System.Collections.Generic;

namespace Pis.Projekt.Business.Scheduling
{
    public interface IScheduledTask<TResult>
    {
        public string Name { get; }
        TResult Result { get; set; }
    }
}