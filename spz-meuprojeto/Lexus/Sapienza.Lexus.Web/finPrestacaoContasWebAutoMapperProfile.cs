using AutoMapper;
using Sapienza.Lexus.finPrestacaoContas.Dtos;
using Sapienza.Lexus.Web.Pages.finPrestacaoContas.ViewModels;

namespace Sapienza.Lexus.Web;

public class finPrestacaoContasWebAutoMapperProfile : Profile
{
    public finPrestacaoContasWebAutoMapperProfile()
    {
        CreateMap<finPrestacaoContasDto, EditfinPrestacaoContasViewModel>();
        CreateMap<CreatefinPrestacaoContasViewModel, CreateUpdatefinPrestacaoContasDto>();
        CreateMap<EditfinPrestacaoContasViewModel, CreateUpdatefinPrestacaoContasDto>();
    }
}
