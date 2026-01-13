using AutoMapper;
using Sapienza.Lexus.finLancamentos.Dtos;

namespace Sapienza.Lexus.finLancamentos;

public class finLancamentosAutoMapperProfile : Profile
{
    public finLancamentosAutoMapperProfile()
    {
        CreateMap<finLancamentos, finLancamentosDto>();
        CreateMap<CreateUpdatefinLancamentosDto, finLancamentos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinLancamentosDto, finLancamentos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
