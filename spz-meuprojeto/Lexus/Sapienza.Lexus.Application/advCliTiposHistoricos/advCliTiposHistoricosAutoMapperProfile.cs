using AutoMapper;
using Sapienza.Lexus.advCliTiposHistoricos.Dtos;

namespace Sapienza.Lexus.advCliTiposHistoricos;

public class advCliTiposHistoricosAutoMapperProfile : Profile
{
    public advCliTiposHistoricosAutoMapperProfile()
    {
        CreateMap<advCliTiposHistoricos, advCliTiposHistoricosDto>();
        CreateMap<CreateUpdateadvCliTiposHistoricosDto, advCliTiposHistoricos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCliTiposHistoricosDto, advCliTiposHistoricos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
