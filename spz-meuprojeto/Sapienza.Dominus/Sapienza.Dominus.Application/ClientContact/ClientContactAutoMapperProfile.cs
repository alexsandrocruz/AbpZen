using AutoMapper;
using Sapienza.Dominus.ClientContact.Dtos;

namespace Sapienza.Dominus.ClientContact;

public class ClientContactAutoMapperProfile : Profile
{
    public ClientContactAutoMapperProfile()
    {
        CreateMap<ClientContact, ClientContactDto>()
            .ForMember(dest => dest.ClientDisplayName, opt => opt.MapFrom(src => src.Client.Name));
        CreateMap<CreateUpdateClientContactDto, ClientContact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateClientContactDto, ClientContact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
