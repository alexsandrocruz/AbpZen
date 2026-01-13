using AutoMapper;
using Sapienza.Lexus.finExtrato.Dtos;

namespace Sapienza.Lexus.finExtrato;

public class finExtratoAutoMapperProfile : Profile
{
    public finExtratoAutoMapperProfile()
    {
        CreateMap<finExtrato, finExtratoDto>();
        CreateMap<CreateUpdatefinExtratoDto, finExtrato>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinExtratoDto, finExtrato>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
