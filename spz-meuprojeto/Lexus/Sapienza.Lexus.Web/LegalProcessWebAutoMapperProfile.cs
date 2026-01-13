using AutoMapper;
using Sapienza.Lexus.LegalProcess.Dtos;
using Sapienza.Lexus.Web.Pages.LegalProcess.ViewModels;

namespace Sapienza.Lexus.Web;

public class LegalProcessWebAutoMapperProfile : Profile
{
    public LegalProcessWebAutoMapperProfile()
    {
        CreateMap<LegalProcessDto, EditLegalProcessViewModel>();
        CreateMap<CreateLegalProcessViewModel, CreateUpdateLegalProcessDto>();
        CreateMap<EditLegalProcessViewModel, CreateUpdateLegalProcessDto>();
    }
}
