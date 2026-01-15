using AutoMapper;
using Sapienza.Dominus.FinancialCategory.Dtos;

namespace Sapienza.Dominus.FinancialCategory;

public class FinancialCategoryAutoMapperProfile : Profile
{
    public FinancialCategoryAutoMapperProfile()
    {
        CreateMap<FinancialCategory, FinancialCategoryDto>();
        CreateMap<CreateUpdateFinancialCategoryDto, FinancialCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateFinancialCategoryDto, FinancialCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
