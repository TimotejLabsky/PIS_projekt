using System.Diagnostics.CodeAnalysis;

namespace Pis.Projekt.Framework
{
    /// <typeparam name="TService">Type of Wsdl Service that uses this configuration</typeparam>
    public class WsdlConfiguration<TService>
    {
        [NotNull]
        public string TeamId { get; set; }
        [NotNull]
        public string Password { get; set; }
    }
}