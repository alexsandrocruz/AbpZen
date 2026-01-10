using AutoMapper;
using Sapienza.Lexus.Client.Dtos;

namespace Sapienza.Lexus.Client;

public class ClientAutoMapperProfile : Profile
{
    public ClientAutoMapperProfile()
    {
        CreateMap<Client, ClientDto>();
        CreateMap<CreateUpdateClientDto, Client>();
        CreateMap<CreateUpdateClientDto, Client>();
    }
}
