using AutoMapper;
using Sapienza.Lexus.Proposal.Dtos;
using Sapienza.Lexus.Web.Pages.Proposal.ViewModels;

namespace Sapienza.Lexus.Web;

public class ProposalWebAutoMapperProfile : Profile
{
    public ProposalWebAutoMapperProfile()
    {
        CreateMap<ProposalDto, EditProposalViewModel>()
            .ForMember(dest => dest.ClientDisplayName, opt => opt.MapFrom(src => src.ClientDisplayName));
        CreateMap<CreateProposalViewModel, CreateUpdateProposalDto>();
        CreateMap<EditProposalViewModel, CreateUpdateProposalDto>();
    }
}
