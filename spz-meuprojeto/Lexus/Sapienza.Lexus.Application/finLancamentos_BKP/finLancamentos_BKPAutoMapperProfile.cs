using AutoMapper;
using Sapienza.Lexus.finLancamentos_BKP.Dtos;

namespace Sapienza.Lexus.finLancamentos_BKP;

public class finLancamentos_BKPAutoMapperProfile : Profile
{
    public finLancamentos_BKPAutoMapperProfile()
    {
        CreateMap<finLancamentos_BKP, finLancamentos_BKPDto>();
        CreateMap<CreateUpdatefinLancamentos_BKPDto, finLancamentos_BKP>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinLancamentos_BKPDto, finLancamentos_BKP>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
