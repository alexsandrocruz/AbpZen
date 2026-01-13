using AutoMapper;
using Sapienza.Lexus.finExtrato.Dtos;
using Sapienza.Lexus.Web.Pages.finExtrato.ViewModels;

namespace Sapienza.Lexus.Web;

public class finExtratoWebAutoMapperProfile : Profile
{
    public finExtratoWebAutoMapperProfile()
    {
        CreateMap<finExtratoDto, EditfinExtratoViewModel>();
        CreateMap<CreatefinExtratoViewModel, CreateUpdatefinExtratoDto>();
        CreateMap<EditfinExtratoViewModel, CreateUpdatefinExtratoDto>();
    }
}
