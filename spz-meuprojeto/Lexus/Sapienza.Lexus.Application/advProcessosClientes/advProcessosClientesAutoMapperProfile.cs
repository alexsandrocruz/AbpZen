using AutoMapper;
using Sapienza.Lexus.advProcessosClientes.Dtos;

namespace Sapienza.Lexus.advProcessosClientes;

public class advProcessosClientesAutoMapperProfile : Profile
{
    public advProcessosClientesAutoMapperProfile()
    {
        CreateMap<advProcessosClientes, advProcessosClientesDto>();
        CreateMap<CreateUpdateadvProcessosClientesDto, advProcessosClientes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProcessosClientesDto, advProcessosClientes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
