using AutoMapper;
using Sapienza.Lexus.finProcuracoesRPV.Dtos;
using Sapienza.Lexus.Web.Pages.finProcuracoesRPV.ViewModels;

namespace Sapienza.Lexus.Web;

public class finProcuracoesRPVWebAutoMapperProfile : Profile
{
    public finProcuracoesRPVWebAutoMapperProfile()
    {
        CreateMap<finProcuracoesRPVDto, EditfinProcuracoesRPVViewModel>();
        CreateMap<CreatefinProcuracoesRPVViewModel, CreateUpdatefinProcuracoesRPVDto>();
        CreateMap<EditfinProcuracoesRPVViewModel, CreateUpdatefinProcuracoesRPVDto>();
    }
}
