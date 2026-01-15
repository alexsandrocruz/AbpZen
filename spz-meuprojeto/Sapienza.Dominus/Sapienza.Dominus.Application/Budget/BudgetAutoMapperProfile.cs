using AutoMapper;
using Sapienza.Dominus.Budget.Dtos;

namespace Sapienza.Dominus.Budget;

public class BudgetAutoMapperProfile : Profile
{
    public BudgetAutoMapperProfile()
    {
        CreateMap<Budget, BudgetDto>()
            .ForMember(dest => dest.FinancialCategoryDisplayName, opt => opt.MapFrom(src => src.FinancialCategory.Name));
        CreateMap<CreateUpdateBudgetDto, Budget>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateBudgetDto, Budget>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
