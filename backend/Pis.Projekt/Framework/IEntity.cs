namespace Pis.Projekt.Domain.Repositories
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}