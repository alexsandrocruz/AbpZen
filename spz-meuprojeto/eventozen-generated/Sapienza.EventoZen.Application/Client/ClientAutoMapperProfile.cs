using AutoMapper;
using Sapienza.EventoZen.Client.Dtos;

namespace Sapienza.EventoZen.Client;

public class ClientAutoMapperProfile : Profile
{
    public ClientAutoMapperProfile()
    {
        CreateMap<Client, ClientDto>();
        CreateMap<CreateUpdateClientDto, Client>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateClientDto, Client>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
