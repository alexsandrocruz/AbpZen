using AutoMapper;
using Sapienza.Dominus.Transaction.Dtos;

namespace Sapienza.Dominus.Transaction;

public class TransactionAutoMapperProfile : Profile
{
    public TransactionAutoMapperProfile()
    {
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.ClientDisplayName, opt => opt.MapFrom(src => src.Client.Name))
            .ForMember(dest => dest.FinancialCategoryDisplayName, opt => opt.MapFrom(src => src.FinancialCategory.Name));
        CreateMap<CreateUpdateTransactionDto, Transaction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateTransactionDto, Transaction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
