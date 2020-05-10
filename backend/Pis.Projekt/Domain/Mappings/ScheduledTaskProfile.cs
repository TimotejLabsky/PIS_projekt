using AutoMapper;
using Pis.Projekt.Api.Responses;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Domain.Mappings
{
    public class ScheduledTaskProfile : Profile
    {
        public ScheduledTaskProfile()
        {
            CreateMap<ScheduledTask, NextTaskResponse>();
            CreateMap<PricedProduct, PricedProductResponse>();
        }
    }
}