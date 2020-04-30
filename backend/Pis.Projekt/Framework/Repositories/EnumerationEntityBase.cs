using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pis.Projekt.Framework.Repositories
{
    public class EnumerationEntityBase<TCode, TName>
        : IEnumerationEntity<TCode, TName>, IEntity<TCode>
    {
        [NotMapped]
        public TCode Id
        {
            get => Code;
            set => Code = value;
        }

        [Key]
        [Column("code")]
        public virtual TCode Code { get; set; }
        
        [Column("name")]
        public virtual TName Name { get; set; }
    }
}