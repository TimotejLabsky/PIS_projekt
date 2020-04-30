namespace Pis.Projekt.Framework.Repositories
{
    public interface IEntity<out TID>
    {
        TID Id { get; }
    }
}