using AutoMapper;
using Sapienza.Lexus.opoOrcamentos.Dtos;
using Sapienza.Lexus.Web.Pages.opoOrcamentos.ViewModels;

namespace Sapienza.Lexus.Web;

public class opoOrcamentosWebAutoMapperProfile : Profile
{
    public opoOrcamentosWebAutoMapperProfile()
    {
        CreateMap<opoOrcamentosDto, EditopoOrcamentosViewModel>();
        CreateMap<CreateopoOrcamentosViewModel, CreateUpdateopoOrcamentosDto>();
        CreateMap<EditopoOrcamentosViewModel, CreateUpdateopoOrcamentosDto>();
    }
}
