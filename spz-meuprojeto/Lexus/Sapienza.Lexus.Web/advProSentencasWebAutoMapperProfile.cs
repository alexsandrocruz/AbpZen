using AutoMapper;
using Sapienza.Lexus.advProSentencas.Dtos;
using Sapienza.Lexus.Web.Pages.advProSentencas.ViewModels;

namespace Sapienza.Lexus.Web;

public class advProSentencasWebAutoMapperProfile : Profile
{
    public advProSentencasWebAutoMapperProfile()
    {
        CreateMap<advProSentencasDto, EditadvProSentencasViewModel>();
        CreateMap<CreateadvProSentencasViewModel, CreateUpdateadvProSentencasDto>();
        CreateMap<EditadvProSentencasViewModel, CreateUpdateadvProSentencasDto>();
    }
}
