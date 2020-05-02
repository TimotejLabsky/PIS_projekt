using AutoMapper;
using Pis.Projekt.Api.Responses;
using Pis.Projekt.Domain.Database;

namespace Pis.Projekt.Domain.Mappings
{
    public class PricedProductProfile: Profile
    {
        public PricedProductProfile()
        {
            CreateMap<PricedProductEntity, PricedProductResponse>();
        }
    }
}