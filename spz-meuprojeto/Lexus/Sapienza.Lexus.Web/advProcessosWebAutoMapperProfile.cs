using AutoMapper;
using Sapienza.Lexus.advProcessos.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProcessosWebAutoMapperProfile : Profile
{
    public advProcessosWebAutoMapperProfile()
    {
        CreateMap<advProcessosDto, EditadvProcessosViewModel>();
        CreateMap<CreateadvProcessosViewModel, CreateUpdateadvProcessosDto>();
        CreateMap<EditadvProcessosViewModel, CreateUpdateadvProcessosDto>();
    }
}
