using AutoMapper;
using Sapienza.Lexus.autoFTP.Dtos;
using Sapienza.Lexus.Web.Pages.autoFTP.ViewModels;

namespace Sapienza.Lexus.Web;

public class autoFTPWebAutoMapperProfile : Profile
{
    public autoFTPWebAutoMapperProfile()
    {
        CreateMap<autoFTPDto, EditautoFTPViewModel>();
        CreateMap<CreateautoFTPViewModel, CreateUpdateautoFTPDto>();
        CreateMap<EditautoFTPViewModel, CreateUpdateautoFTPDto>();
    }
}
