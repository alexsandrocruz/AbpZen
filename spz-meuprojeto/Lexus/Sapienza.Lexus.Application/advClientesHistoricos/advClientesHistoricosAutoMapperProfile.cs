using AutoMapper;
using Sapienza.Lexus.advClientesHistoricos.Dtos;

namespace Sapienza.Lexus.advClientesHistoricos;

public class advClientesHistoricosAutoMapperProfile : Profile
{
    public advClientesHistoricosAutoMapperProfile()
    {
        CreateMap<advClientesHistoricos, advClientesHistoricosDto>();
        CreateMap<CreateUpdateadvClientesHistoricosDto, advClientesHistoricos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvClientesHistoricosDto, advClientesHistoricos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
