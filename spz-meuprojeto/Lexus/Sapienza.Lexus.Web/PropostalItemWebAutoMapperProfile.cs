using AutoMapper;
using Sapienza.Lexus.PropostalItem.Dtos;
using Sapienza.Lexus.Web.Pages.PropostalItem.ViewModels;

namespace Sapienza.Lexus.Web;

public class PropostalItemWebAutoMapperProfile : Profile
{
    public PropostalItemWebAutoMapperProfile()
    {
        CreateMap<PropostalItemDto, EditPropostalItemViewModel>();
        CreateMap<CreatePropostalItemViewModel, CreateUpdatePropostalItemDto>();
        CreateMap<EditPropostalItemViewModel, CreateUpdatePropostalItemDto>();
    }
}
