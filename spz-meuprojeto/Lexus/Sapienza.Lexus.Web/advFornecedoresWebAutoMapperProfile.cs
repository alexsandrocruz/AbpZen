using AutoMapper;
using Sapienza.Lexus.advFornecedores.Dtos;
using Sapienza.Lexus.Web.Pages.advFornecedores.ViewModels;

namespace Sapienza.Lexus.Web;

public class advFornecedoresWebAutoMapperProfile : Profile
{
    public advFornecedoresWebAutoMapperProfile()
    {
        CreateMap<advFornecedoresDto, EditadvFornecedoresViewModel>();
        CreateMap<CreateadvFornecedoresViewModel, CreateUpdateadvFornecedoresDto>();
        CreateMap<EditadvFornecedoresViewModel, CreateUpdateadvFornecedoresDto>();
    }
}
