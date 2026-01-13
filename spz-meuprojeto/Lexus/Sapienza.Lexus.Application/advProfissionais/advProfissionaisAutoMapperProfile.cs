using AutoMapper;
using Sapienza.Lexus.advProfissionais.Dtos;

namespace Sapienza.Lexus.advProfissionais;

public class advProfissionaisAutoMapperProfile : Profile
{
    public advProfissionaisAutoMapperProfile()
    {
        CreateMap<advProfissionais, advProfissionaisDto>();
        CreateMap<CreateUpdateadvProfissionaisDto, advProfissionais>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProfissionaisDto, advProfissionais>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
