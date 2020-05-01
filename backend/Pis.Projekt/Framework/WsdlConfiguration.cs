namespace Pis.Projekt.Framework
{
    /// <typeparam name="TService">Type of Wsdl Service that uses this configuration</typeparam>
    public class WsdlConfiguration<TService>
    {
        public string TeamId { get; set; }
        public string Password { get; set; }
    }
}