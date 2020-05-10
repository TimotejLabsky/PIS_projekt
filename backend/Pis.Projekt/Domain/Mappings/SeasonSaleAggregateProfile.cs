using AutoMapper;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Domain.Mappings
{
    public class SeasonSaleAggregateProfile : Profile
    {
        public SeasonSaleAggregateProfile()
        {
            CreateMap<SeasonPricedProductEntity, SeasonSaleAggregate>();
        }
    }
}