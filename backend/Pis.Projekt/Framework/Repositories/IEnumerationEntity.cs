namespace Pis.Projekt.Framework.Repositories
{
    public interface IEnumerationEntity<TCode, TName>
    {
        TCode Code { get; set; }
        TName Name { get; set; }
    }

    public interface IEnumerationEntity
        : IEnumerationEntity<string, string>
    {
        // empty
    }
}