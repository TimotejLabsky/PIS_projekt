using AutoMapper;
using Pis.Projekt.Api.Responses;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Domain.Mappings
{
    public class SalesAggregateProfile: Profile
    {
        public SalesAggregateProfile()
        {
            CreateMap<SalesAggregateEntity, SalesAggregate>()
                .ForMember(d => d.Product, o => o.MapFrom(s => s.Product));
            
            CreateMap<SalesAggregate, SalesAggregateEntity>();
            CreateMap<ProductEntity, Product>();
        }
    }
}