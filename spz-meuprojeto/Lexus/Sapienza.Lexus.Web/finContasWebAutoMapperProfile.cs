using AutoMapper;
using Sapienza.Lexus.finContas.Dtos;
using Sapienza.Lexus.Web.Pages.finContas.ViewModels;

namespace Sapienza.Lexus.Web;

public class finContasWebAutoMapperProfile : Profile
{
    public finContasWebAutoMapperProfile()
    {
        CreateMap<finContasDto, EditfinContasViewModel>();
        CreateMap<CreatefinContasViewModel, CreateUpdatefinContasDto>();
        CreateMap<EditfinContasViewModel, CreateUpdatefinContasDto>();
    }
}
