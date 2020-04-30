using System.Threading.Tasks;

namespace Pis.Projekt.Business.Scheduling
{
    public interface ITask<TResult>
    {
        string Name { get; }
        TResult Result { get; set; }
    }
}