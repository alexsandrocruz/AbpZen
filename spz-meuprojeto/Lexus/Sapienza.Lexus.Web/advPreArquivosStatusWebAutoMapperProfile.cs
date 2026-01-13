using AutoMapper;
using Sapienza.Lexus.advPreArquivosStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advPreArquivosStatus.ViewModels;

namespace Sapienza.Lexus.Web;

public class advPreArquivosStatusWebAutoMapperProfile : Profile
{
    public advPreArquivosStatusWebAutoMapperProfile()
    {
        CreateMap<advPreArquivosStatusDto, EditadvPreArquivosStatusViewModel>();
        CreateMap<CreateadvPreArquivosStatusViewModel, CreateUpdateadvPreArquivosStatusDto>();
        CreateMap<EditadvPreArquivosStatusViewModel, CreateUpdateadvPreArquivosStatusDto>();
    }
}
