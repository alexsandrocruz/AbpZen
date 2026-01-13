using AutoMapper;
using Sapienza.Lexus.advClientes.Dtos;

namespace Sapienza.Lexus.advClientes;

public class advClientesAutoMapperProfile : Profile
{
    public advClientesAutoMapperProfile()
    {
        CreateMap<advClientes, advClientesDto>();
        CreateMap<CreateUpdateadvClientesDto, advClientes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvClientesDto, advClientes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
