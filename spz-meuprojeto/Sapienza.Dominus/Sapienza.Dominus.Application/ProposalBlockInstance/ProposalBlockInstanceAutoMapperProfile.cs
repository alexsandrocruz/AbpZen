using AutoMapper;
using Sapienza.Dominus.ProposalBlockInstance.Dtos;

namespace Sapienza.Dominus.ProposalBlockInstance;

public class ProposalBlockInstanceAutoMapperProfile : Profile
{
    public ProposalBlockInstanceAutoMapperProfile()
    {
        CreateMap<ProposalBlockInstance, ProposalBlockInstanceDto>()
            .ForMember(dest => dest.ProposalDisplayName, opt => opt.MapFrom(src => src.Proposal.Title));
        CreateMap<CreateUpdateProposalBlockInstanceDto, ProposalBlockInstance>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateProposalBlockInstanceDto, ProposalBlockInstance>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
