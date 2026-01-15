using AutoMapper;
using Sapienza.Dominus.SmsLog.Dtos;

namespace Sapienza.Dominus.SmsLog;

public class SmsLogAutoMapperProfile : Profile
{
    public SmsLogAutoMapperProfile()
    {
        CreateMap<SmsLog, SmsLogDto>();
        CreateMap<CreateUpdateSmsLogDto, SmsLog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateSmsLogDto, SmsLog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
