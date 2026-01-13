using AutoMapper;
using Sapienza.Lexus._versaoBD.Dtos;
using Sapienza.Lexus.Web.Pages._versaoBD.ViewModels;

namespace Sapienza.Lexus.Web;

public class _versaoBDWebAutoMapperProfile : Profile
{
    public _versaoBDWebAutoMapperProfile()
    {
        CreateMap<_versaoBDDto, Edit_versaoBDViewModel>();
        CreateMap<Create_versaoBDViewModel, CreateUpdate_versaoBDDto>();
        CreateMap<Edit_versaoBDViewModel, CreateUpdate_versaoBDDto>();
    }
}
