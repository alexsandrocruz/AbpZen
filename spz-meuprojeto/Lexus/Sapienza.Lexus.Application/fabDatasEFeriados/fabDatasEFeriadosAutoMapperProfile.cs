using AutoMapper;
using Sapienza.Lexus.fabDatasEFeriados.Dtos;

namespace Sapienza.Lexus.fabDatasEFeriados;

public class fabDatasEFeriadosAutoMapperProfile : Profile
{
    public fabDatasEFeriadosAutoMapperProfile()
    {
        CreateMap<fabDatasEFeriados, fabDatasEFeriadosDto>();
        CreateMap<CreateUpdatefabDatasEFeriadosDto, fabDatasEFeriados>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabDatasEFeriadosDto, fabDatasEFeriados>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
