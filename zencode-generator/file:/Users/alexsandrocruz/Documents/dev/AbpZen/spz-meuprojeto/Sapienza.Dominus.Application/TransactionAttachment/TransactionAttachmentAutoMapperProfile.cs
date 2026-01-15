using AutoMapper;
using Sapienza.Dominus.TransactionAttachment.Dtos;

namespace Sapienza.Dominus.TransactionAttachment;

public class TransactionAttachmentAutoMapperProfile : Profile
{
    public TransactionAttachmentAutoMapperProfile()
    {
        CreateMap<TransactionAttachment, TransactionAttachmentDto>()
            .ForMember(dest => dest.TransactionDisplayName, opt => opt.MapFrom(src => src.Transaction.Description));
        CreateMap<CreateUpdateTransactionAttachmentDto, TransactionAttachment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateTransactionAttachmentDto, TransactionAttachment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
