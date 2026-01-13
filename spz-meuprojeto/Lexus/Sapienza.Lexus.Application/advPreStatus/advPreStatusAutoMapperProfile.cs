using AutoMapper;
using Sapienza.Lexus.advPreStatus.Dtos;

namespace Sapienza.Lexus.advPreStatus;

public class advPreStatusAutoMapperProfile : Profile
{
    public advPreStatusAutoMapperProfile()
    {
        CreateMap<advPreStatus, advPreStatusDto>();
        CreateMap<CreateUpdateadvPreStatusDto, advPreStatus>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPreStatusDto, advPreStatus>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
