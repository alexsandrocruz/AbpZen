#nullable enable
using AutoMapper;
using Sapienza.Lexus.Proposal.Dtos;

namespace Sapienza.Lexus.Proposal;

public class ProposalAutoMapperProfile : Profile
{
    public ProposalAutoMapperProfile()
    {
        CreateMap<Proposal, ProposalDto>()
            .ForMember(dest => dest.ClientDisplayName, opt => opt.MapFrom(src => src.Client != null ? src.Client.Name : null));
        CreateMap<CreateUpdateProposalDto, Proposal>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
