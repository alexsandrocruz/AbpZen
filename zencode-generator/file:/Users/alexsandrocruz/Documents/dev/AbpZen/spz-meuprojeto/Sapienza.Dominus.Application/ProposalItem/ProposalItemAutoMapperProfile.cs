using AutoMapper;
using Sapienza.Dominus.ProposalItem.Dtos;

namespace Sapienza.Dominus.ProposalItem;

public class ProposalItemAutoMapperProfile : Profile
{
    public ProposalItemAutoMapperProfile()
    {
        CreateMap<ProposalItem, ProposalItemDto>()
            .ForMember(dest => dest.ProposalDisplayName, opt => opt.MapFrom(src => src.Proposal.Title));
        CreateMap<CreateUpdateProposalItemDto, ProposalItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateProposalItemDto, ProposalItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
