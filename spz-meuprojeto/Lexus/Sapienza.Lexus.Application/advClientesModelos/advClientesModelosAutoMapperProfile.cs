using AutoMapper;
using Sapienza.Lexus.advClientesModelos.Dtos;

namespace Sapienza.Lexus.advClientesModelos;

public class advClientesModelosAutoMapperProfile : Profile
{
    public advClientesModelosAutoMapperProfile()
    {
        CreateMap<advClientesModelos, advClientesModelosDto>();
        CreateMap<CreateUpdateadvClientesModelosDto, advClientesModelos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvClientesModelosDto, advClientesModelos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
