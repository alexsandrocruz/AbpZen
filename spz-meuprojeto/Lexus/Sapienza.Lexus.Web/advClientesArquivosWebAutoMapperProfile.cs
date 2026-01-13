using AutoMapper;
using Sapienza.Lexus.advClientesArquivos.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesArquivos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advClientesArquivosWebAutoMapperProfile : Profile
{
    public advClientesArquivosWebAutoMapperProfile()
    {
        CreateMap<advClientesArquivosDto, EditadvClientesArquivosViewModel>();
        CreateMap<CreateadvClientesArquivosViewModel, CreateUpdateadvClientesArquivosDto>();
        CreateMap<EditadvClientesArquivosViewModel, CreateUpdateadvClientesArquivosDto>();
    }
}
