using AutoMapper;
using Sapienza.Lexus.finLancamentos.Dtos;
using Sapienza.Lexus.Web.Pages.finLancamentos.ViewModels;

namespace Sapienza.Lexus.Web;

public class finLancamentosWebAutoMapperProfile : Profile
{
    public finLancamentosWebAutoMapperProfile()
    {
        CreateMap<finLancamentosDto, EditfinLancamentosViewModel>();
        CreateMap<CreatefinLancamentosViewModel, CreateUpdatefinLancamentosDto>();
        CreateMap<EditfinLancamentosViewModel, CreateUpdatefinLancamentosDto>();
    }
}
