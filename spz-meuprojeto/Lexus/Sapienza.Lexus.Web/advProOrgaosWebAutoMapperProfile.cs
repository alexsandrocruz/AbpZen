using AutoMapper;
using Sapienza.Lexus.advProOrgaos.Dtos;
using Sapienza.Lexus.Web.Pages.advProOrgaos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProOrgaosWebAutoMapperProfile : Profile
{
    public advProOrgaosWebAutoMapperProfile()
    {
        CreateMap<advProOrgaosDto, EditadvProOrgaosViewModel>();
        CreateMap<CreateadvProOrgaosViewModel, CreateUpdateadvProOrgaosDto>();
        CreateMap<EditadvProOrgaosViewModel, CreateUpdateadvProOrgaosDto>();
    }
}
