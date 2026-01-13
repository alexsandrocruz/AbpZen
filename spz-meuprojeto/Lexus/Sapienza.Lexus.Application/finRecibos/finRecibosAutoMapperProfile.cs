using AutoMapper;
using Sapienza.Lexus.finRecibos.Dtos;

namespace Sapienza.Lexus.finRecibos;

public class finRecibosAutoMapperProfile : Profile
{
    public finRecibosAutoMapperProfile()
    {
        CreateMap<finRecibos, finRecibosDto>();
        CreateMap<CreateUpdatefinRecibosDto, finRecibos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinRecibosDto, finRecibos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
