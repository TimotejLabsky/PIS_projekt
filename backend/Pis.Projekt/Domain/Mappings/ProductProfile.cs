using AutoMapper;
using Pis.Projekt.Api.Responses;
using Pis.Projekt.Domain.Database;

namespace Pis.Projekt.Domain.Mappings
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductEntity, ProductResponse>();
        }
    }
}