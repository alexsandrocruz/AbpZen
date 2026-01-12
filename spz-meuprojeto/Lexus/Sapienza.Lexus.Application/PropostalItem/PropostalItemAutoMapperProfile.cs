using AutoMapper;
using Sapienza.Lexus.PropostalItem.Dtos;

namespace Sapienza.Lexus.PropostalItem;

public class PropostalItemAutoMapperProfile : Profile
{
    public PropostalItemAutoMapperProfile()
    {
        CreateMap<PropostalItem, PropostalItemDto>()
            .ForMember(dest => dest.ProposalDisplayName, opt => opt.MapFrom(src => src.Proposal.Number));
        CreateMap<CreateUpdatePropostalItemDto, PropostalItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatePropostalItemDto, PropostalItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
