using AutoMapper;
using Testing.Domain.DTOs.Orders;
using Testing.Domain.DTOs.Products;
using Testing.Domain.Entities;

namespace Testing.Services.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Specification,
                opt => opt.MapFrom(src => src.PcbSpecifications.FirstOrDefault()));

        CreateMap<PcbSpecification, PCBSpecificationDTO>();

        CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.ProductName, opt => opt.Ignore())
            .ForMember(dest => dest.StatusText, opt => opt.Ignore());
    }
}