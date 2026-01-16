using AutoMapper;
using Sapienza.Lexus.advCliTiposHistoricos.Dtos;

namespace Sapienza.Lexus.advCliTiposHistoricos;

public class advCliTiposHistoricosAutoMapperProfile : Profile
{
    public advCliTiposHistoricosAutoMapperProfile()
    {
        CreateMap<advCliTiposHistoricos, advCliTiposHistoricosDto>()
            .ForMember(dest => dest.advClientesHistoricosDisplayName, opt => opt.MapFrom(src => src.advCliTiposHistoricosNav.data));
        CreateMap<CreateUpdateadvCliTiposHistoricosDto, advCliTiposHistoricos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCliTiposHistoricosDto, advCliTiposHistoricos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
