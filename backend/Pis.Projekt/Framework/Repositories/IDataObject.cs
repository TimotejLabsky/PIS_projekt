namespace Pis.Projekt.Framework.Repositories
{
    public interface IDataObject<out TID>
    {
        public TID Id { get; }
    }
}