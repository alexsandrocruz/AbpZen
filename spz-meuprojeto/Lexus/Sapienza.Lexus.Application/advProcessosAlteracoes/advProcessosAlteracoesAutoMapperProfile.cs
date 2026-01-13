using AutoMapper;
using Sapienza.Lexus.advProcessosAlteracoes.Dtos;

namespace Sapienza.Lexus.advProcessosAlteracoes;

public class advProcessosAlteracoesAutoMapperProfile : Profile
{
    public advProcessosAlteracoesAutoMapperProfile()
    {
        CreateMap<advProcessosAlteracoes, advProcessosAlteracoesDto>();
        CreateMap<CreateUpdateadvProcessosAlteracoesDto, advProcessosAlteracoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProcessosAlteracoesDto, advProcessosAlteracoes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
