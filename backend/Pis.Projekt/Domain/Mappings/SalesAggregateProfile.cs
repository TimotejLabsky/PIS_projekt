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
            CreateMap<SalesAggregateEntity, SalesAggregateResponse>();
            CreateMap<SalesAggregateEntity, SalesAggregate>();
        }
    }
}