using AutoMapper;
using Sapienza.Lexus.advCliCargos.Dtos;

namespace Sapienza.Lexus.advCliCargos;

public class advCliCargosAutoMapperProfile : Profile
{
    public advCliCargosAutoMapperProfile()
    {
        CreateMap<advCliCargos, advCliCargosDto>();
        CreateMap<CreateUpdateadvCliCargosDto, advCliCargos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCliCargosDto, advCliCargos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
