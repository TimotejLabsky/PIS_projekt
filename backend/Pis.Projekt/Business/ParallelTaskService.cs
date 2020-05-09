using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pis.Projekt.Business
{
    public class ParallelTaskService
    {
        public static async Task ExecuteAsync(params Task[] tasks)
        { 
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}