using AutoMapper;
using Sapienza.Dominus.ClientMessage.Dtos;

namespace Sapienza.Dominus.ClientMessage;

public class ClientMessageAutoMapperProfile : Profile
{
    public ClientMessageAutoMapperProfile()
    {
        CreateMap<ClientMessage, ClientMessageDto>()
            .ForMember(dest => dest.ClientDisplayName, opt => opt.MapFrom(src => src.Client.Name));
        CreateMap<CreateUpdateClientMessageDto, ClientMessage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateClientMessageDto, ClientMessage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
