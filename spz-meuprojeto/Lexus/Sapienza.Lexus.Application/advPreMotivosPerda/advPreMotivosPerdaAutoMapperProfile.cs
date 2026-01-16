using AutoMapper;
using Sapienza.Lexus.advPreMotivosPerda.Dtos;

namespace Sapienza.Lexus.advPreMotivosPerda;

public class advPreMotivosPerdaAutoMapperProfile : Profile
{
    public advPreMotivosPerdaAutoMapperProfile()
    {
        CreateMap<advPreMotivosPerda, advPreMotivosPerdaDto>();
        CreateMap<CreateUpdateadvPreMotivosPerdaDto, advPreMotivosPerda>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPreMotivosPerdaDto, advPreMotivosPerda>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
