using AutoMapper;
using Sapienza.Dominus.WhatsappLog.Dtos;

namespace Sapienza.Dominus.WhatsappLog;

public class WhatsappLogAutoMapperProfile : Profile
{
    public WhatsappLogAutoMapperProfile()
    {
        CreateMap<WhatsappLog, WhatsappLogDto>();
        CreateMap<CreateUpdateWhatsappLogDto, WhatsappLog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateWhatsappLogDto, WhatsappLog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
