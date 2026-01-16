using AutoMapper;
using Sapienza.Lexus.advCliLog.Dtos;

namespace Sapienza.Lexus.advCliLog;

public class advCliLogAutoMapperProfile : Profile
{
    public advCliLogAutoMapperProfile()
    {
        CreateMap<advCliLog, advCliLogDto>();
        CreateMap<CreateUpdateadvCliLogDto, advCliLog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCliLogDto, advCliLog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
