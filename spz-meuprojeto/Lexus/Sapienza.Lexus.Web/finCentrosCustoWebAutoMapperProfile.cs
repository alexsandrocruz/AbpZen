using AutoMapper;
using Sapienza.Lexus.finCentrosCusto.Dtos;
using Sapienza.Lexus.Web.Pages.finCentrosCusto.ViewModels;

namespace Sapienza.Lexus.Web;

public class finCentrosCustoWebAutoMapperProfile : Profile
{
    public finCentrosCustoWebAutoMapperProfile()
    {
        CreateMap<finCentrosCustoDto, EditfinCentrosCustoViewModel>();
        CreateMap<CreatefinCentrosCustoViewModel, CreateUpdatefinCentrosCustoDto>();
        CreateMap<EditfinCentrosCustoViewModel, CreateUpdatefinCentrosCustoDto>();
    }
}
