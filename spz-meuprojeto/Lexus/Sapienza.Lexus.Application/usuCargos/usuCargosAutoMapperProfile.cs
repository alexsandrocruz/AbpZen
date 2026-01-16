using AutoMapper;
using Sapienza.Lexus.usuCargos.Dtos;

namespace Sapienza.Lexus.usuCargos;

public class usuCargosAutoMapperProfile : Profile
{
    public usuCargosAutoMapperProfile()
    {
        CreateMap<usuCargos, usuCargosDto>();
        CreateMap<CreateUpdateusuCargosDto, usuCargos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateusuCargosDto, usuCargos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
