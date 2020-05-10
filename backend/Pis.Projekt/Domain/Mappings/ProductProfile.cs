using AutoMapper;
using Pis.Projekt.Api.Responses;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Domain.Mappings
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductEntity, ProductResponse>();
            CreateMap<Product, ProductEntity>();
        }
    }
}