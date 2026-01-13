using AutoMapper;
using Sapienza.Lexus.advCliTiposArquivos.Dtos;
using Sapienza.Lexus.Web.Pages.advCliTiposArquivos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advCliTiposArquivosWebAutoMapperProfile : Profile
{
    public advCliTiposArquivosWebAutoMapperProfile()
    {
        CreateMap<advCliTiposArquivosDto, EditadvCliTiposArquivosViewModel>();
        CreateMap<CreateadvCliTiposArquivosViewModel, CreateUpdateadvCliTiposArquivosDto>();
        CreateMap<EditadvCliTiposArquivosViewModel, CreateUpdateadvCliTiposArquivosDto>();
    }
}
