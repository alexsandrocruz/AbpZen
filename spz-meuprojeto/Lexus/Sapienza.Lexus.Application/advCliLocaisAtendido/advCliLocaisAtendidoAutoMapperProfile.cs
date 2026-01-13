using AutoMapper;
using Sapienza.Lexus.advCliLocaisAtendido.Dtos;

namespace Sapienza.Lexus.advCliLocaisAtendido;

public class advCliLocaisAtendidoAutoMapperProfile : Profile
{
    public advCliLocaisAtendidoAutoMapperProfile()
    {
        CreateMap<advCliLocaisAtendido, advCliLocaisAtendidoDto>();
        CreateMap<CreateUpdateadvCliLocaisAtendidoDto, advCliLocaisAtendido>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCliLocaisAtendidoDto, advCliLocaisAtendido>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
