using AutoMapper;
using Sapienza.Lexus.advProStatus.Dtos;

namespace Sapienza.Lexus.advProStatus;

public class advProStatusAutoMapperProfile : Profile
{
    public advProStatusAutoMapperProfile()
    {
        CreateMap<advProStatus, advProStatusDto>();
        CreateMap<CreateUpdateadvProStatusDto, advProStatus>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProStatusDto, advProStatus>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
