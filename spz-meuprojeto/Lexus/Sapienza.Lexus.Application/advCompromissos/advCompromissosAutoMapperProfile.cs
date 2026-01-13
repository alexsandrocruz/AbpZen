using AutoMapper;
using Sapienza.Lexus.advCompromissos.Dtos;

namespace Sapienza.Lexus.advCompromissos;

public class advCompromissosAutoMapperProfile : Profile
{
    public advCompromissosAutoMapperProfile()
    {
        CreateMap<advCompromissos, advCompromissosDto>();
        CreateMap<CreateUpdateadvCompromissosDto, advCompromissos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCompromissosDto, advCompromissos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
