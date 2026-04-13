using AutoMapper;
using Testing.Domain.DTOs.Products;
using Testing.Domain.Entities;

namespace Testing.Services.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDTO>();
        CreateMap<PcbSpecification, PCBSpecificationDTO>();
    }
}