using AutoMapper;
using Sapienza.Dominus.Client.Dtos;

namespace Sapienza.Dominus.Client;

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
