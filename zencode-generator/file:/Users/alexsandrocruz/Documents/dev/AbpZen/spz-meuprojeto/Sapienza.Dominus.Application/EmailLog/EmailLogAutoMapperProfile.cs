using AutoMapper;
using Sapienza.Dominus.EmailLog.Dtos;

namespace Sapienza.Dominus.EmailLog;

public class EmailLogAutoMapperProfile : Profile
{
    public EmailLogAutoMapperProfile()
    {
        CreateMap<EmailLog, EmailLogDto>();
        CreateMap<CreateUpdateEmailLogDto, EmailLog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateEmailLogDto, EmailLog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
