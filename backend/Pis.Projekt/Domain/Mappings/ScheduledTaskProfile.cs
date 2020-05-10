using System;
using System.Linq;
using AutoMapper;
using Pis.Projekt.Api.Requests;
using Pis.Projekt.Api.Responses;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Domain.Mappings
{
    public class ScheduledTaskProfile : Profile
    {
        public ScheduledTaskProfile()
        {
            CreateMap<ScheduledTask, NextTaskResponse>()
                .ForMember(d => d.Products,
                    o =>
                        o.MapFrom(s => s.Products.Select(p =>
                            new NextTaskResponse.TaskProductResponse
                            {
                                Price = p.Price,
                                SalesWeek = p.SalesWeek,
                                SoldAmount = p.SoldAmount,
                                Id = p.Id,
                                Name = p.Product.Name,
                                ProductId = p.Product.Id,
                                SaleCoefficient = p.SaleCoefficient
                            })))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));
            CreateMap<TaskProduct, PricedProductResponse>();
            CreateMap<TaskFulfillRequest.TaskProductRequest, TaskProduct>();
        }
    }
}