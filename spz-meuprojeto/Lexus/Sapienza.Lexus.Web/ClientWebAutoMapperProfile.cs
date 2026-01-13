using AutoMapper;
using Sapienza.Lexus.Client.Dtos;
using Sapienza.Lexus.Web.Pages.Client.ViewModels;

namespace Sapienza.Lexus.Web;

public class ClientWebAutoMapperProfile : Profile
{
    public ClientWebAutoMapperProfile()
    {
        CreateMap<ClientDto, EditClientViewModel>();
        CreateMap<CreateClientViewModel, CreateUpdateClientDto>();
        CreateMap<EditClientViewModel, CreateUpdateClientDto>();
    }
}
