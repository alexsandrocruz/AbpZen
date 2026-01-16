using AutoMapper;
using Sapienza.Lexus.fabMotivosPerda.Dtos;

namespace Sapienza.Lexus.fabMotivosPerda;

public class fabMotivosPerdaAutoMapperProfile : Profile
{
    public fabMotivosPerdaAutoMapperProfile()
    {
        CreateMap<fabMotivosPerda, fabMotivosPerdaDto>();
        CreateMap<CreateUpdatefabMotivosPerdaDto, fabMotivosPerda>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabMotivosPerdaDto, fabMotivosPerda>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
