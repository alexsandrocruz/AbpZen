using AutoMapper;
using Sapienza.Lexus.advRevisaoDocumentos.Dtos;
using Sapienza.Lexus.Web.Pages.advRevisaoDocumentos.ViewModels;

namespace Sapienza.Lexus.Web;

public class advRevisaoDocumentosWebAutoMapperProfile : Profile
{
    public advRevisaoDocumentosWebAutoMapperProfile()
    {
        CreateMap<advRevisaoDocumentosDto, EditadvRevisaoDocumentosViewModel>();
        CreateMap<CreateadvRevisaoDocumentosViewModel, CreateUpdateadvRevisaoDocumentosDto>();
        CreateMap<EditadvRevisaoDocumentosViewModel, CreateUpdateadvRevisaoDocumentosDto>();
    }
}
