using AutoMapper;
using Sapienza.Lexus.advProSentencas.Dtos;

namespace Sapienza.Lexus.advProSentencas;

public class advProSentencasAutoMapperProfile : Profile
{
    public advProSentencasAutoMapperProfile()
    {
        CreateMap<advProSentencas, advProSentencasDto>();
        CreateMap<CreateUpdateadvProSentencasDto, advProSentencas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProSentencasDto, advProSentencas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
