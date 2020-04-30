using Newtonsoft.Json;

namespace Pis.Projekt.Framework.Repositories
{
    public class EnumerationDataObjectBase<TCode, TName>
        : IDataObject<TCode>
    {
        [JsonIgnore]
        public TCode Id
        {
            get => Code;
            set => Code = value;
        }

        [JsonProperty("code")]
        public virtual TCode Code { get; set; }
        
        [JsonProperty("name")]
        public virtual TName Name { get; set; }
    }
}